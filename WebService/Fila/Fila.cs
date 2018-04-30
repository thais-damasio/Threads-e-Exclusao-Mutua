using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.FilaDados
{
    class Fila
    {
        /*Apontador para célula da frente da fila e para trás da fila.*/
        Celula frente;
        Celula tras;
        /*Tamanho da fila.*/
        int tamanho;

        public Fila()
        {
            /*Criação de sentinela.*/
            Celula temp = new Celula();
            frente = temp;
            tras = temp;
            tamanho = 0;
        }
        
        /*Retorna 'true' para fila vazia e 'false' para não vazia.*/
        public bool Vazia()
        {
            return tamanho == 0;
        }
        /*Enfileira dados no final da fila.*/
        public void Enfileirar(int dado)
        {
            Celula celula = new Celula(null,dado);
            tras.prox = celula;
            tras = celula;
            tamanho++;
        }
        /*Retira o primeiro da fila e rtorna '-1' para fila vazia.*/
        public int Desenfileirar()
        {
            if (Vazia())
            {
                return -1;
            }
            int dado = frente.prox.dado;
            frente = frente.prox;
            tamanho--;
            return dado;
        }
        /*Retorna tamanho da fila.*/
        public int GetTamanho()
        {
            return tamanho;
        }
    }
}
