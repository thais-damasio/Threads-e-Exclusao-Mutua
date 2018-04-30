using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using WebService.FilaDados;

namespace WebService
{
    class Servidor
    {
        /*Pool de Threads, onde ficarão armazenadas as requisições.*/
        static Thread[] pool = new Thread[15];
        /*Fila utilizada para guardar os indices do pool que estao livres para armazenar requisições.*/
        public static Fila fila = new Fila();
        /*contador de threads perdidas.*/
        public static int perdidas;
        /*contador de thread atendidas*/
        public static int atendidas;
        /*contador de threads finalizadas*/
        public static int finalizadas;
        /*contador de thread iniciadas*/
        public static int inicializadas;

        static void Main(string[] args)
        {
            /*Quando o programa inicia ja cria-se o arquivo de saída.*/
            FileStream arquivo = new FileStream("Out.txt", FileMode.Create);
            arquivo.Close();

            /*A fila é preenchida com os índices do pool (A fila sempre deve conter os mesmos indices segundo o tamanho do pool).*/
            Console.WriteLine("=========================");
            Console.WriteLine("Disparando requisições...");
            Console.WriteLine("=========================");
            for (int i = 0; i < pool.Length; i++)
            {
                fila.Enfileirar(i);
            }

            /*Dispara o metodo de gerar requisição a cada 1 segundo durante o intervalo do for.*/
            for (int i = 1; i <= 60; i++)
            {
                CriarRequisicao(i);
                Console.Title = "Tempo: " + i;
                Thread.Sleep(1000);
            }

            /*Após o término da geração de requisições aguarda até a ultima thread ser executada, quando a fila tem o mesmo
             * tamanho do pool significa que todos os índices estão disponíveis.*/
            Console.WriteLine("=========================\n" +
                "Aguardando a última thread ser atendida...." +
                "\n=========================");
            while (fila.GetTamanho() != pool.Length)
            {
                //Espera
            }

            /*Ao final do programa é impresso a quantidade de threads negadas, atendidas e finalizadas.*/
            Console.WriteLine("=========================");
            Console.WriteLine("======= RESULTADO =======");
            Console.WriteLine("=========================");
            Console.WriteLine("Negadas: " + perdidas);
            Console.WriteLine("Atendidas: " + finalizadas);
            Console.WriteLine("Inicializadas: " + inicializadas);
            Console.WriteLine("Concluídas: " + finalizadas);
            Console.ReadKey();
        }

        /*Metodo que simula a requisição feita ao Servidor Web.*/
        static void CriarRequisicao(int id)
        { 
            /*Se a fila possuir algum valor significa que há uma posição vazia, então é gerada uma requisição
             *caso contrário a requisição é negada.*/
            if (!fila.Vazia())
            {
                atendidas++;
                /*Cada requisição possui como ID o indice da fila que esta "desocupado".*/
                Requisicao requisicao = new Requisicao(id, fila.Desenfileirar());
                /*O índice "desocupado" corresponde a posição desocupada no pool. Cria-se uma thread para atender a requisição
                 *e a starta logo em seguida.*/
                pool[requisicao.indice] = new Thread(requisicao.Requerir);
                pool[requisicao.indice].Start();
            }
            else
            {
                /*Contador de requisições que foram perdidas, pois o pool de requisições estava cheio.*/
                perdidas++;
            }
        }

    }
}
