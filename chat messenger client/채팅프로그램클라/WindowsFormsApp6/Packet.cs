using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public class Packet
    {
        public StringBuilder sb; 

        public void SetData(string _msg)
        {
            sb.Append(_msg);
        }
        public void SetData(string _nickname,string _id,string _pass,string _email)
        {
            sb.Append(_nickname);
            sb.Append(" ");
            sb.Append(_id);
            sb.Append(" ");
            sb.Append(_pass);
            sb.Append(" ");
            sb.Append(_email);
        }
        public void SetData(string _id,string _password)
        {
            sb.Append(_id);
            sb.Append(" ");
            sb.Append(_password);
        }
         
        public byte[] GetData
        {
            get
            {
                return Encoding.UTF8.GetBytes(sb.ToString());
            }
        }
    }

    public class aGenerationPacket : Packet
    {
        public aGenerationPacket()
        { 
            sb = new StringBuilder(cmd.aGeneration + " ");
        }
    }

    public class bGenerationPacket : Packet
    {
        public bGenerationPacket()
        {
            sb = new StringBuilder(cmd.bGeneration + " ");
        }
    }

    public class cGenerationPacket : Packet
    {
        public cGenerationPacket()
        {
            sb = new StringBuilder(cmd.cGeneration + " ");
        }   
    }
    public class dGenerationPacket : Packet
    {
        public dGenerationPacket()
        {
            sb = new StringBuilder(cmd.dGeneration + " ");
        }
    }
    public class eGenerationPacket : Packet
    {
        public eGenerationPacket()
        {
            sb = new StringBuilder(cmd.eGeneration + " ");
        }
    }

    public class fGenerationPacket : Packet
    {
        public fGenerationPacket()
        {
            sb = new StringBuilder(cmd.fGeneration + " ");
        }
    }

    public class lGenerationPacket : Packet
    {
        public lGenerationPacket()
        {
            sb = new StringBuilder(cmd.lGeneration + " ");
        }
    }
    public class xGenerationPacket : Packet
    {
        public xGenerationPacket()
        {
            sb = new StringBuilder(cmd.xGeneration + " ");
        }
    }
    public class tGenerationPacket : Packet
    {
        public tGenerationPacket()
        {
            sb = new StringBuilder(cmd.tGeneration + " ");
        }
    } 
    static public class cmd
    {
        public const string aGeneration = "a"; // 이메일 전송
        public const string bGeneration = "b"; // 인증번호 전송
        public const string cGeneration = "c"; // 회원정보 전송
        public const string dGeneration = "d"; // 아이디 중복확인
        public const string eGeneration = "e"; // 강제종료
        public const string fGeneration = "f"; // 온라인친구요청
        public const string lGeneration = "l"; // 로그인 정보 전송
        public const string xGeneration = "x"; // 사용종료
        public const string tGeneration = "t"; // 할말 전달
    }
}
