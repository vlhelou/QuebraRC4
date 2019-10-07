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


            string hash = "3B54676D6B0DE3A0CA95983B562146604AEC49D401C4B7B67363F73A9D157D86FDE9956A4947E0B549A72CEFC698F98BBC6D52987662F776B8B59DA7AF2F74E5854AFEB8B96BFC379CCE631B83331AE3897EC9E282E2C878057CC6CF0B00CFE7757E72DA04B1E4457B66FEAC4D326791BB44648C21415AE1D2ED4EB49179CE10B40F0CE8A9263E55FF2145CB74C3603BAD659C55FF2C681834691D06BF7AEB4CDFC03DC1D35F91BC01C0264370584B3B968FAECCD434BAED4B1D96F9FC75F93EBB1E03A52C1443F7D7373A6EE28B21B3CFE157B4C2E92A61AA1264D7";
            string texto = "A sexta-feira (4) é de metal no Rock In Rio e basta uma olhada rápida para perceber que, por aqui, preto e camisetas de banda, principalmente do Iron Maiden, são a ordem do dia na Cidade do Rock. Mesmo assim, muita gente";
            byte[] arTexto = Encoding.ASCII.GetBytes(texto);
            byte[] arAlvo = StringToByteArray(hash);
            //ulong inicio = 4400000;
            ulong inicio = UltimoValor();

            ulong ends = 1099511627776;
            //1099511627776
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

                    /*Parallel.For(0, 1000000, index =>
                    {

                    });*/
                    if (start % 100000 == 0)
                    {
                        Console.WriteLine(start.ToString("N0"));
                        file.WriteLine($"{DateTime.Now}; {start} ");
                        file.Flush();

                    }

                }

            }

            Console.WriteLine("fim");
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
            byte[] arHash = clRC4.Encrypt(tentativa, TextoOrigem);
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
                string UltimaLinha=string.Empty;
                while ((linha = Arquivo.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(linha))
                        UltimaLinha = linha;
                }
                Arquivo.Close();
                if (!string.IsNullOrEmpty(UltimaLinha)){
                    string[] valores = UltimaLinha.Split(";");
                    if (valores.Length==2){
                        ulong.TryParse(valores[1].Trim(), out retorno);
                        
                    }
                }
            }
            return retorno;
        }
    }
}
