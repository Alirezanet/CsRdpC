using System;
using Xunit;

namespace CsRdpC.Core.Tests {
   public class ClientX224ConnectionRequestPDUShould {
      public ClientX224ConnectionRequestPDU client { get; set; }
      public ClientX224ConnectionRequestPDUShould () {
         client = new ClientX224ConnectionRequestPDU ();
      }

      [Theory]
      [InlineData (129)]
      [InlineData (65531)]
      [InlineData (7)]
      [InlineData (0)]
      [InlineData (65532)]
      public void GetTpktHeader_Should (UInt16 TPDU_size) {
         if (TPDU_size < 7 || TPDU_size > 65531)
            Assert.Throws<Exception> (() => client.GetTpktHeader (TPDU_size));
         else {
            var tpktHeader = client.GetTpktHeader (TPDU_size);
            Assert.Equal (4, tpktHeader.Length);
            Assert.Equal (0b00000011, tpktHeader[0]);
            Assert.Equal (0b00000000, tpktHeader[1]);

            var packLen = BitConverter.GetBytes ((UInt16) (TPDU_size + 4));
            Assert.Equal (2, packLen.Length);
            Assert.Equal (packLen[0], tpktHeader[2]);
            Assert.Equal (packLen[1], tpktHeader[3]);
         }
      }

      [Fact]
      public void GetX244Crq_Should () {
         var x = client.GetX244Crq ();
         Assert.Equal (7, x.Length);
      }

   }
}