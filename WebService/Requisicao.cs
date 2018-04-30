using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace WebService
{
    class Requisicao
    {
        /*Objetos que serão usados nos monitores.*/
        static object inicializar = new object();
        static object finalizar = new object();
        static object arquivo = new object();

        /*Get de tempo que define um tempo randomico para as Threads de 3 a 7.*/
        public int tempo
        {
            get
            {
                Random tempo = new Random();
                return tempo.Next(3, 8);
            }
            private set
            {
                tempo = value;
            }
        }
        /*Id de identificação que corresponde ao índice do pool.*/
        public int indice { get; private set; }
        /*Id de identificação que corresponde ao índice do pool.*/
        public int id { get; private set; }

        public Requisicao(int id, int indice)
        {
            this.id = id;
            this.indice = indice;
        }

        /*Método que simula o pedido da requisição sendo atendido.*/
        public void Requerir()
        {
            int tempo = this.tempo;
            Console.WriteLine("R" + id + " iniciando (pool[" + indice + "]) | Tempo:" + tempo + ".");

            /*Monitor para cuidar da manipulação de arquivos. Aqui escreve-se os detalhes de início de thread.*/
            Monitor.Enter(arquivo);
            StreamWriter escreve = new StreamWriter("Out.txt", true);
            escreve.WriteLine("R" + id + ",iniciando," + Servidor.inicializadas + "," + tempo);
            escreve.Close();
            Monitor.Exit(arquivo);

            /*Monitor para cuidar da manipulação da váriavel 'inicializadas'. Aqui incrementa a mesma.*/
            Monitor.Enter(inicializar);
            Servidor.inicializadas++;
            Monitor.Exit(inicializar);

            /*Pausa da thread*/
            Thread.Sleep(tempo * 1000);

            /*Monitor para cuidar da manipulação de arquivos. Aqui escreve-se os detalhes de fim de thread*/
            Monitor.Enter(arquivo);
            escreve = new StreamWriter("Out.txt", true);
            escreve.WriteLine("R" + id + ",concluindo," + Servidor.finalizadas + "," + tempo);
            escreve.Close();
            Monitor.Exit(arquivo);

            /*Monitor para cuidar da manipulação da variável 'finalizadas'. Aqui incrementa a mesma.*/
            Monitor.Enter(finalizar);
            Servidor.finalizadas++;
            Monitor.Exit(finalizar);

            Console.WriteLine("R" + id + " concluindo (pool[" + indice + "]) | Tempo:" + tempo + ".");
            /*Leva para fila o id, indicando o fim da execução e que o índice está liberado para a entrada de nova 
             *requisição.*/
            Servidor.fila.Enfileirar(indice);
        }
    }
}
