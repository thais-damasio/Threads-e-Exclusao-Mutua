using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.FilaDados
{
    class Celula
    {
        /*Apontador para próxima célula.*/
        public Celula prox;
        /*Dado da célula.*/
        public int dado;

        /*Construtor para criação de sentinela.*/
        public Celula(){ }
        public Celula (Celula prox, int dado)
        {
            this.prox = prox;
            this.dado = dado;
        }         
    }
}
