using System;

namespace CsRdpC.Core {
   public class ClientX224ConnectionRequestPDU {
      /// <summary>
      /// A TPKT consists of two parts: a packet header, followed by a TPDU. The format of the packet
      /// header is constant, independent of the type of TPDU. The packet header consists of four octets
      /// 
      /// Octet 1:   Octet 1 is a version number, with binary value 0000 0011. 
      /// Octet 2:   Octet 2 is reserved for further study.
      /// Octet 3-4: Octet 3-4 unsigned 16-bit binary encoding of the TPKT length. This is the length of the
      ///            entire packet in octets, including both the packet header and the TPDU.
      /// 
      /// specified in [T123] section 8.
      /// </summary>
      /// <param name="TPDU_packet_size">the length of the TPDU packets</param>
      /// <returns>4 Octet (byte[]) TPTK header</returns>
      public byte[] GetTpktHeader (UInt16 TPDU_packet_size) {
         if (TPDU_packet_size < 7 || TPDU_packet_size > 65531)
            throw new Exception ("Invalid packet lenght. the packet size must bet between 7-65531 octets");
         var psize_bytes = BitConverter.GetBytes ((UInt16) (TPDU_packet_size + 4));
         return new byte[4] { 3, 0, psize_bytes[0], psize_bytes[1] };
      }

      /// <summary>
      /// Connection Request (CR) TPDU
      /// 
      /// The structure of this part shall contain:
      /// a) CR – Connection request code: 1110. Bits 8 to 5 of octet 2.
      /// b) CDT – Initial credit allocation (set to 0000 in classes 0 and 1 when specified as preferred class). Bits 4 to 1 of octet 2.
      /// c) DST-REF – Set to zero.
      /// d) SRC-REF – Reference selected by the transport entity initiating the CR-TPDU to identify the requested transport connection.
      /// e) CLASS OPTION – Bits 8 to 5 of octet 7 define the preferred transport protocol class to be operated over the requested transport connection.
      ///    When operating over CONS, this field shall take one of the following
      /// values:
      ///  0000 Class 0;
      ///  0001 Class 1;
      ///  0010 Class 2;
      ///  0011 Class 3;
      ///  0100 Class 4.
      /// When operating over CLNS, this field shall take the value 0100 to indicate class 4.
      /// </summary>
      /// <returns>(7 byte Crq)</returns>
      public byte[] GetX244Crq () {
         // The field is contained in the first octet of the TPDUs. The length is indicated by a binary number, with a maximum value
         // of 254 (1111 1110). The length indicated shall be the header length in octets including parameters, but excluding the
         // length indicator field and user data, if any.
         byte LI = 254;
         return new byte[] {
            LI,
            224, // CR - CDT
            0, 0, // DST-REF
            0, 0, // SRC-REF
            0     // class options : class 0 
         };
      }
   }
}