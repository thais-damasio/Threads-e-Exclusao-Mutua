# Threads e Exclusão Mútua
PONTIFÍCIA UNIVERSIDADE CATÓLICA DE MINAS GERAIS – PUC/MG<br>
Disciplina de Sistemas Operacionais<br>
<b>Professor:</b> Lesandro Ponciano<br>
<b>Desenvolvedoras:</b> Jéssica Mota Dias e Thais Regina Damásio

# Introdução
<p>Este trabalho tem como objetivo analisar o funcionamento de um sistema operacional, simulando como este trabalha com o gerenciamento de programas, processos e tarefas, através de uma simulação de requisições feitas em um servidor web. No presente trabalho pode-se observar conceitos aprendidos em sala de aula sendo implementados na prática, como o paralelismo, sistemas multithreading, sincronização de processos, exclusão mútua dentre outros.</p>
<p>Os meios utilizados para construir o cenário foram a biblioteca System.Threading (com suas classes Monitor e Thread) e uma estrutura de dado do tipo Fila, para facilitar a manipulação dos índices do pool de threads simulado, pois ao invés de percorrer todo o pool utilizou-se a Fila para guardar os índices disponíveis do mesmo. Realizou-se testes de analises dos comportamentos das threads e monitores em diferentes cenários, para avaliar o desempenho e a eficiência de tais aplicações. Ao final pode-se identificar os desafios encontrados para implementar esse cenário e as soluções descobertas para suprir essas dificuldades.</p>

# Ideias e Bibliotecas utilizadas
<p>A biblioteca System.Threading foi usada pois fornece classes e interfaces que permitem a programação multithreaded, nesse caso a utilizamos para fazer uso das classes Thread e Monitor (classe para sincronizar atividades de threads).</p>

###  Monitor
<p>O  monitor fornece um mecanismo que sincroniza o acesso a objetos, produzindo um bloqueio no objeto especificado em sua instancia, restringindo o acesso a um bloco de código (sessão critica). No código implementado o monitor foi utilizado, pois as threads não poderiam acessar as variáveis "inicializar" e "finalizar" ao mesmo tempo. Tais variáveis contam quantas threads foram iniciadas e quantas foram finalizadas, se duas threads fizessem o acesso simultaneamente ocorreria perca de valor em tais objetos, e os valores apresentados estariam errados, por isso o uso do monitor.</p>
<p>O monitor também foi utilizado para a escrita no arquivo texto, pois, como era apenas um arquivo de escrita (Out.txt) para indicar as variáveis inicializadas e as concluídas, poderia ocorrer conflito, pois se a thread A acessa o arquivo quando inicia e a B tenta acessa-lo ao mesmo tempo quando conclui ocorrerá conflito, mesmo tendo um monitor para inicializadas e concluídas, portanto consideramos utilizar um monitor para arquivo, que sincronizava o acesso ao mesmo, então quando uma thread é iniciada e deseja escrever no arquivo texto, antes ela passa pelo monitor de arquivo que verifica se há alguma thread escrevendo no arquivo, se não houver, ela é liberada pra escrever seu estado e seguir para a próxima parte do código.</p>
<p> -	Monitor.Enter: Fornece um bloqueio exclusivo no objeto especificado, por exemplo “Monitor.Enter(inicializadas);”  foi invocado o uso do monitor na variável “inicializadas”.<br>
    -	Monitor.Exit: Libera um bloqueio exclusivo no objeto especificado, o método Exit realiza a função contraria do método Enter, ele libera a variável para ser acessada.</p>
    
### Thread
<p>A classe Thread cria e controla todos os métodos de uma thread (formas de um processo dividir a si mesmo em duas ou mais tarefas) nesse caso o suporte a Thread foi fornecido através da biblioteca System.Threading, mencionada anteriormente nesse trabalho.
Na implementação foi utilizado um “vetor” do tipo Thread que simulava um pool de Threads, onde todas as requisições (threads) ficavam armazenadas.</p>
<p>- Thread.Sleep: Suspende o thread no numero especificado de milissegundos. No código implementado uma requisição deveria ser gerada a cada 1 segundo durante 60 segundos, portanto utilizamos o método Sleep para simular a espera, uma requisição era criada, a Thread principal no caso o Servidor “dormia” por 1 segundo e logo após outra requisição era gerada (foi utilizada a estrutura de repetição For nessa tarefa).</p>
<p>Na construção do trabalho cogitou-se usar a classe Timer, pois poderia ser simulado um clock, que a cada segundo geraria uma requisição no servidor. O Timer seria uma boa opção nesse sistema, porem o código não seria tão otimizado, pois enquanto o Timer estaria trabalhando, haveria uma estrutura de repetição na Main que verificaria a todo momento se o Timer já estava finalizado, isso poderia gerar gasto de CPU desnecessário, por isso optou-se por utilizar outros métodos que foram mencionados anteriormente nesse trabalho. Conclui-se que o Timer é uma opção viável e eficiente em determinados contextos, no entanto verificou-se que nesta aplicação tal método não se apresentou como o mais eficiente.</p>

# Testes
<p> Na implementação de exclusão mútua foi escolhida a classe Monitor, comprovando a necessidade de exclusão mútua: </p>
<p><b>●	1º Teste:</b> Foi implementado um sistema simples que cada Thread deve acrescentar 10.000.000 a variável de controle, o resultado final esperado seria de 15.000.000, já que foram disparadas 15 Threads. Sem o uso de exclusão mútua existe perca de valor já que duas ou mais threads poderiam acessar a mesma sessão simultaneamente capturando um valor da variável de controle que ainda estaria sendo atualizada por outra Thread. </p>

![Teste 1](https://uploaddeimagens.com.br/images/001/396/749/full/teste_1.png?1525115621)

<p><b>●	2º Teste:</b> Este teste consistia em entender o comportamento das threads, com a adição de um monitor no código de simulação de servidor web. A ideia consistiu em utilizar um pool com suporte a três requisições, estas seriam disparadas a cada 1 segundo durante 15 segundos, o método que elas executaram forneceram um resultado que auxiliou no entendimento de como elas se comportam. Quando uma thread entra no monitor, as que chegam depois precisam esperar a conclusão daquela que já está no monitor e de fato isso acontece como mostrado logo abaixo. Note, que quando a thread deixa o monitor a outra consegue ter acesso. Com esse teste além de entendermos o comportamento do monitor, foi possível visualizar o comportamento do pool de threads que só liberava o espaço quando uma thread de fato era concluída. </p>

![Teste 2](https://uploaddeimagens.com.br/images/001/396/761/full/teste_2_%281%29.png?1525115849)

![Teste 2 - 2](https://uploaddeimagens.com.br/images/001/396/763/full/teste_2_%282%29.png?1525115959)

<p>O entendimento de como funciona o monitor nos auxiliou na percepção de erros que poderiam ser causados pela falta de exclusão mútua. Quando foi implementado a funcionalidade de escrita em arquivo texto, onde cada Thread no início e no fim da execução deveria escrever seu status. Percebemos a importância do uso da exclusão mútua para o modelo de código que estávamos formulando, visto que dois processos não podem alterar um arquivo de texto ao mesmo tempo, sem a exclusão mútua elas tentariam abrir o mesmo arquivo ao mesmo tempo e geraria um erro em tempo de execução que iria interromper o programa.</p>

# Dificuldades
<p>A maior dificuldade encontrada durante a elaboração do trabalho foi realizar testes e entender o comportamento das Threads, visto que não seria possível utilizar o recurso de execução linha a linha fornecido pela IDE que utilizamos, sendo difícil identificar a execução de uma Thread, pois essas iniciam paralelamente.</p>

# Conclusão
<p>Concluímos que o sistema operacional trabalha a todo o momento de forma que o uso de suas diversas funcionalidades seja otimizado. Através de aplicações multithreading pode-se perceber na pratica como funciona o comportamento do mesmo. Trabalhando com gerenciamento de threads na simulação de um servidor web observou-se como funciona os diversos conceitos apresentados na disciplina Sistemas Operacionais. Quando o programa foi dividido em diversos módulos aplicou-se o paralelismo, tornando o processo mais rápido e eficiente, pode-se citar também como beneficio do paralelismo a multitarefa, pois quando as threads subdividem o programa, os processos não ficam estáticos esperando que outro termine, fazendo uso de vários recursos do sistema. </p>
<p>Percebe-se também a sincronização dos processos feita pelo Sistema Operacional através do uso do Monitor que trabalha com exclusão mutua, organizando o acesso a determinados métodos e variáveis, evitando condições de corrida, pois dois processos não irão acessar uma sessão critica ao mesmo tempo, aprimorando o paralelismo.</p>
<p>Os testes também foram essenciais para o melhor entendimento de cada aplicação, através da pratica entende-se com mais intensidade o que é apresentado na disciplina de Sistemas Operacionais. Por fim pode-se compreender o funcionamento de um sistema multithreading e as ferramentas utilizadas para o gerenciamento de tal, podendo-se aplicar na pratica o conhecimento teórico adquirido na presente disciplina.</p>


## Referencias
- MICROSOFT. Classe Thread. Disponível em: 
<https://msdn.microsoft.com/pt-br/library/system.threading.thread(v=vs.110).aspx>. Acesso em 19 Abr. 2018.
- MICROSOFT. Classe Monitor. Disponível em: 
<https://msdn.microsoft.com/pt-br/library/system.threading.monitor(v=vs.110).aspx>. Acesso em 19 Abr. 2018.
- MICROSOFT. Classe Timer. Disponível em: 
<https://msdn.microsoft.com/pt-br/library/system.timers.timer(v=vs.110).aspx>. Acesso em 19 Abr. 2018.
- MICROSOFT. Namespace System.Threading. Disponível em: 
<https://msdn.microsoft.com/pt-br/library/system.threading(v=vs.110).aspx>. Acesso em 21 Abr. 2018
