using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuebraRC4
{
    class Program
    {
        static void Main(string[] args)
        {
            //ulong maximo = 9223372036854775807;
            //long outro = 296443535898841000000000000;
            // byte[] teste = BitConverter.GetBytes(18223372036854775807);

            // sucesso: [146][152][238][21][15][0][0][0] 
            //

            byte[] senha = { 146, 152, 238, 21, 15 };

            byte[] Tentativatexto = Encoding.ASCII.GetBytes("hoje e o meu dia e que dia mais feliz");




            byte[] resultado =  clRC4.Decrypt(senha, Tentativatexto);
            Console.WriteLine(BitConverter.ToString(resultado));

            //string hash = "1C017D287215EB";
            //string texto = "fui ali";

            //byte[] arTexto = Encoding.ASCII.GetBytes(texto);
            //byte[] arAlvo = StringToByteArray(hash);
            //ulong inicio = 4400000;
            //ulong inicio = UltimoValor();

            //ulong ends = 1099511627776;
            //1099511627776
            /*
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt", true))
            {
                for (ulong start = inicio; start < ends; start++)
                {
                    if (VerificaChave(
                        arAlvo,
                        BitConverter.GetBytes(start),
                        arTexto,
                        file
                        ))
                    {
                        return;
                    }
                    if (start % 1000000 == 0)
                    {
                        Console.WriteLine(start.ToString("N0"));
                        file.WriteLine($"{DateTime.Now}; {start} ");
                        file.Flush();
                    }
                }
            }
            */
            Console.WriteLine("fim");
            Console.ReadKey();
        }


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        static string PrintValor(byte[] valor)
        {
            StringBuilder retorno = new StringBuilder();
            foreach (byte bt in valor)
            {
                retorno.AppendFormat("[{0}]", bt);
            }
            return retorno.ToString();
        }

        static byte[] CriaArray()
        {
            byte[] retorno = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                retorno[i] = (byte)i;
            }
            return retorno;
        }

        static byte[] LimpaZeros(byte[] origem)
        {
            int PontCorte = 0;
            for (int i = origem.Length - 1; i > 0; i--)
            {
                if (origem[i] == 0)
                {
                    PontCorte = i;
                }
                else
                {
                    break;
                }
            }
            byte[] retorno = new byte[PontCorte];
            for (int i = 0; i < PontCorte; i++)
            {
                retorno[i] = origem[i];
            }
            return retorno;
        }


        static bool VerificaChave(byte[] Alvo, byte[] tentativa, byte[] TextoOrigem, System.IO.StreamWriter log)
        {
            byte[] ar40bt = new byte[5];
            ar40bt[0] = tentativa[0];
            ar40bt[1] = tentativa[1];
            ar40bt[2] = tentativa[2];
            ar40bt[3] = tentativa[3];
            ar40bt[4] = tentativa[4];
            byte[] arHash = clRC4.Encrypt(ar40bt, TextoOrigem);
            if (arHash.SequenceEqual(Alvo))
            {
                Console.WriteLine("functionou");
                log.WriteLine("Funcionou");
                log.WriteLine($"{DateTime.Now}; {PrintValor(tentativa)} ");
                return true;
            }
            return false;
        }

        static ulong UltimoValor()
        {
            ulong retorno = 0;
            if (System.IO.File.Exists("log.txt"))
            {

                System.IO.StreamReader Arquivo = new System.IO.StreamReader("log.txt");
                string linha;
                string UltimaLinha = string.Empty;
                while ((linha = Arquivo.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(linha))
                        UltimaLinha = linha;
                }
                Arquivo.Close();
                if (!string.IsNullOrEmpty(UltimaLinha))
                {
                    string[] valores = UltimaLinha.Split(";");
                    if (valores.Length == 2)
                    {
                        ulong.TryParse(valores[1].Trim(), out retorno);

                    }
                }
            }
            return retorno;
        }
    }
}
