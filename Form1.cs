using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using API_OrgaoRegulador;
using System.Net.Mail;
using MySql.Data;
using MySql.Data.MySqlClient;
//Adriano Araújo dos Reis Botega - RA 13005335
//Giovana Moraes da Silva - RA 15158058
//Marcelo Monteiro da Silva Valente - RA 15246903


namespace megatime
{
    public partial class Form1 : Form
    {
        private EndPoint endPoint;
        //parametros
        List<Int32> dezenas = new List<Int32>();//para inserir as dezenas selecionadas
        List<Int32> times = new List<Int32>();//para inserir os times selecionados individualmente
        List<Int32> allTimes = new List<Int32>();//para inserir todos os times para calculo de vezes jogadas
        List<Button> listButton = new List<Button>();//para inserir os botoes
        List<PictureBox> listTime = new List<PictureBox>();//para inserir a imagem dos times

        double valor = 0, valorBase = 0, valorA = 0, valorB = 0, valorC = 0, valorTimes = 0, valorPagar = 0;
        bool enviarAposta = false, executeJogo = true;
        int time, i;
        string resultApsotado;
        public Form1()
        {
            //instanciando o objeto endPoint da classe EndPoint
            endPoint = new EndPoint();
            InitializeComponent();
            //criando listButton
            for (i = 0; i <= 99; i++)
            {
                listButton.Add(b00); listButton.Add(b01); listButton.Add(b02); listButton.Add(b03); listButton.Add(b04); listButton.Add(b05); listButton.Add(b06); listButton.Add(b07); listButton.Add(b08); listButton.Add(b09); listButton.Add(b10); listButton.Add(b11); listButton.Add(b12); listButton.Add(b13); listButton.Add(b14); listButton.Add(b15); listButton.Add(b16); listButton.Add(b17); listButton.Add(b18); listButton.Add(b19); listButton.Add(b20); listButton.Add(b21); listButton.Add(b22); listButton.Add(b23); listButton.Add(b24); listButton.Add(b25); listButton.Add(b26); listButton.Add(b27); listButton.Add(b28); listButton.Add(b29); listButton.Add(b30); listButton.Add(b31); listButton.Add(b32); listButton.Add(b33); listButton.Add(b34); listButton.Add(b35); listButton.Add(b36); listButton.Add(b37); listButton.Add(b38); listButton.Add(b39); listButton.Add(b40); listButton.Add(b41); listButton.Add(b42); listButton.Add(b43); listButton.Add(b44); listButton.Add(b45); listButton.Add(b46); listButton.Add(b47); listButton.Add(b48); listButton.Add(b49); listButton.Add(b50); listButton.Add(b51); listButton.Add(b52); listButton.Add(b53); listButton.Add(b54); listButton.Add(b55); listButton.Add(b56); listButton.Add(b57); listButton.Add(b58); listButton.Add(b59); listButton.Add(b60); listButton.Add(b61); listButton.Add(b62); listButton.Add(b63); listButton.Add(b64); listButton.Add(b65); listButton.Add(b66); listButton.Add(b67); listButton.Add(b68); listButton.Add(b69); listButton.Add(b70); listButton.Add(b71); listButton.Add(b72); listButton.Add(b73); listButton.Add(b74); listButton.Add(b75); listButton.Add(b76); listButton.Add(b77); listButton.Add(b78); listButton.Add(b79); listButton.Add(b80); listButton.Add(b81); listButton.Add(b82); listButton.Add(b83); listButton.Add(b84); listButton.Add(b85); listButton.Add(b86); listButton.Add(b87); listButton.Add(b88); listButton.Add(b89); listButton.Add(b90); listButton.Add(b91); listButton.Add(b92); listButton.Add(b93); listButton.Add(b94); listButton.Add(b95); listButton.Add(b96); listButton.Add(b97); listButton.Add(b98); listButton.Add(b99);
            }
            //criando listTime
            for (i = 0; i <= 25; i++)
            {
                listTime.Add(time00); listTime.Add(time01); listTime.Add(time02); listTime.Add(time03); listTime.Add(time04); listTime.Add(time05); listTime.Add(time06); listTime.Add(time07); listTime.Add(time08); listTime.Add(time09); listTime.Add(time10); listTime.Add(time11); listTime.Add(time12); listTime.Add(time13); listTime.Add(time14); listTime.Add(time15); listTime.Add(time16); listTime.Add(time17); listTime.Add(time18); listTime.Add(time19); listTime.Add(time20); listTime.Add(time21); listTime.Add(time22); listTime.Add(time23); listTime.Add(time24); listTime.Add(time25);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        //esta função serve para escolher o time a partir da dezena apostada
        private int changeTime(int id)
        {
            if (id == 0)
            {
                time = 25;
            }
            else
            {
                time = id / 4;

                if (id % 4 != 0)
                {
                    time = time + 1;
                }
            }
            return time;
        }
        //esta função é para quando clicar no botão da dezena
        private void runButton(object sender, EventArgs e)
        {
            if (executeJogo == true)
            {
                Button buttonName = (Button)sender;
                Int32 dezena = Int32.Parse(buttonName.Text);
                startFunc(dezena);
            }

        }
        //começa a fazer função de selecionar
        private void startFunc(int dezena)
        {
            if (executeJogo == true)
            {
                bool exist = dezenas.Contains(dezena);
                if (exist == true)
                {
                    removeNumber(dezena);
                }
                else
                {
                    addNumber(dezena);
                }
                checkStatus(dezena);
            }
        }
        //esta função serve para checar as regras do jogo
        private void checkStatus(Int32 dezena)
        {
            //checando dezenas
            int qtdeDezenas = dezenas.Count();
            int chkDezenas = 0, chkTimes = 0;
            if (qtdeDezenas >= 10)
            {
                imgRegra1.Image = Image.FromFile(@"..\..\img\right.png");
                chkDezenas = 1;
                valorBase = 5;
                if (qtdeDezenas > 20)
                {
                    MessageBox.Show("Você excedeu o limite de 20 dezenas");
                    removeNumber(dezena);
                }
                else
                {
                    if (qtdeDezenas >= 10 && qtdeDezenas <= 15)
                    {
                        valorA = (qtdeDezenas - 10) * 0.75;
                        valor = valorBase + valorA;
                    }
                    if (qtdeDezenas > 15 && qtdeDezenas <= 19)
                    {
                        valorB = (qtdeDezenas - 15) * 3;
                        valor = valorBase + valorA + valorB;
                    }
                    if (qtdeDezenas == 20)
                    {
                        valorC = 7;
                        valor = valorBase + valorA + valorB + valorC;
                    }
                }
            }
            else
            {
                imgRegra1.Image = Image.FromFile(@"..\..\img\wrong.png");
                chkDezenas = 0;
                label27.Text = ("R$ 0,00");
            }
            //checando times
            chooseTeam(dezena);
            int qtdeTimes = times.Count();
            if (qtdeTimes >= 5)
            {
                imgRegra2.Image = Image.FromFile(@"..\..\img\right.png");
                chkTimes = 1;
                int countTimes = times.Count();
                if (countTimes > 5)
                {
                    valorTimes = ((countTimes - 5) * 1.25);
                }
                else
                {
                    valorTimes = 0;
                }
            }
            else
            {
                imgRegra2.Image = Image.FromFile(@"..\..\img\wrong.png");
                chkTimes = 0;
            }
            //verificacao final
            if ((chkDezenas + chkTimes) >= 2)
            {
                btnEnviar.ForeColor = Color.Black;
                btnEnviar.Cursor = Cursors.Hand;
                enviarAposta = true;
                tipEnviar.SetToolTip(btnEnviar, "Enviar aposta");
            }
            else
            {
                btnEnviar.ForeColor = SystemColors.AppWorkspace;
                btnEnviar.Cursor = Cursors.No;
                enviarAposta = false;
                tipEnviar.SetToolTip(btnEnviar, "Ainda não é possível enviar a aposta");
            }
            valorPagar = valor + valorTimes;
            label27.Text = "R$ " + (valorPagar).ToString("0.00");
        }
        //remove a dezena
        private void removeNumber(Int32 dezena)
        {
            dezenas.Remove(dezena);
            allTimes.Remove(changeTime(dezena));
            listButton[dezena].BackColor = Color.Transparent;
            listButton[dezena].FlatStyle = FlatStyle.Standard;
            log.Text = "Removida dezena " + dezena.ToString("00") + "\n" + log.Text;
        }
        //adiciona a dezena
        private void addNumber(Int32 dezena)
        {
            allTimes.Add(changeTime(dezena));
            dezenas.Add(dezena);
            listButton[dezena].BackColor = Color.SlateGray;
            listButton[dezena].FlatStyle = FlatStyle.Popup;
            log.Text = "Adicionada dezena " + dezena.ToString("00") + "\n" + log.Text;
        }
        //faz os procedimentos quando o time é escolhido
        private void chooseTeam(Int32 id)
        {
            if (dezenas.Contains(id))
            {
                listTime[changeTime(id)].Image = Image.FromFile(@"..\..\img\" + changeTime(id) + ".png");
                if (!times.Contains(changeTime(id)))
                {
                    times.Add(changeTime(id));
                }
            }
            else
            {
                if (!allTimes.Contains(changeTime(id)))
                {
                    times.Remove(changeTime(id));
                    listTime[changeTime(id)].Image = Image.FromFile(@"..\..\img\" + changeTime(id) + "off.png");
                }
            }
        }
        //botao de enviar a aposta
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (enviarAposta == true)
            {
                //parametros
                log.Text = "Conectando ao órgão regulador...\n\n" + log.Text;
                log.Text = "Eviando a aposta...\n\n" + log.Text;
                string numerosApostados = "";
                long resultado;
                dezenas.Sort();
                //montando string com dezenas apostadas
                for (i = 0; i < dezenas.Count(); i++)
                {
                    numerosApostados = numerosApostados + "," + dezenas[i].ToString("00");
                }
                numerosApostados = numerosApostados.Remove(0, 1);
                //enviando string para o orgao
                resultado = endPoint.gravarAposta(numerosApostados);
                if (resultado != 0)
                {
                    printarJogo(resultado, 1);
                    toSendMail.Visible = true;
                    executeJogo = false;
                }
                else
                {
                    log.Text = "Erro ao enviar a aposta!\n\n" + log.Text;
                }
            }
        }
        //printar na tela
        private void printarJogo(long resultado, int act)
        {
            //verificando os times apostados
            allTimes.Sort();
            int lastTime = 0, qtdetime = 1;
            string resultTimeApostado = "";
            string nameTime = "";
            String numerosApostados = endPoint.obterTodasDezenasApostadas(resultado);
            foreach (int timeApostado in allTimes)
            {
                //altera o numero do time pelo nome
                nameTime = numberToTime(timeApostado);
                //fim trocar int por string do time
                if (timeApostado == lastTime)
                {
                    resultTimeApostado = resultTimeApostado.Replace(nameTime + "(" + qtdetime + ")\n", "");
                    qtdetime++;
                    resultTimeApostado = resultTimeApostado + nameTime + "(" + qtdetime + ")\n";
                }
                else
                {
                    qtdetime = 1;
                    resultTimeApostado = resultTimeApostado + nameTime + "(" + qtdetime + ")\n";
                }
                nameTime = "";
                lastTime = timeApostado;
            }
            resultApsotado = "";
            resultApsotado = resultApsotado + "MEGATIME\n-----------------\n";
            if (act == 2)
            {
                resultApsotado = resultApsotado + "SOMENTE CONSULTA\n\n";
            }
            resultApsotado = resultApsotado + "Protocolo\n" + resultado + "\n\n";
            resultApsotado = resultApsotado + "Dezenas apostadas\n" + numerosApostados + "\n\n";
            if (act == 1)
            {
                resultApsotado = resultApsotado + "Times apostados\n" + resultTimeApostado + "\n\n";
                resultApsotado = resultApsotado + "Valor da aposta\nR$ " + valorPagar.ToString("###,##0.00") + "\n\n";
                btnEnviar.ForeColor = SystemColors.AppWorkspace;
                btnEnviar.Cursor = Cursors.No;
                enviarAposta = false;
                tipEnviar.SetToolTip(btnEnviar, "Aposta enviada com sucesso");
            }
            log.Text = resultApsotado;
        }
        //botao para abrir painel de busca
        private void buscar_Click(object sender, EventArgs e)
        {
            executeJogo = false;
            executeLimpar();
            panelMainExecute.Visible = false;
            panelSearch.Visible = true;
            protocoloNum.Focus();
        }
        //botao para retornar ao painel principal
        private void backPanelMain_Click(object sender, EventArgs e)
        {
            executeJogo = true;
            voltaPaineis();
            if (protocoloNum.Text != "")
            {
                log.Text = "";
            }
            protocoloNum.Clear();
            executeLimpar();
        }
        //fechar painel de email
        private void backFromEmail_Click(object sender, EventArgs e)
        {
            toSendMail.Visible = false;
            voltaPaineis();
        }
        //metodo voltar paineis
        private void voltaPaineis()
        {
            panelSearch.Visible = false;
            panelMail.Visible = false;
            panelMainExecute.Visible = true;
        }
        //verifica sorteio
        private void verificarSorteio_Click(object sender, EventArgs e)
        {
            executeJogo = true;
            executeLimpar();
            string numProtocolo = protocoloNum.Text;
            if (numProtocolo == "")
            {
                MessageBox.Show("Digite o número do protocolo");
                protocoloNum.Focus();
            }
            else
            {
                String dezenasApostadas = endPoint.obterTodasDezenasApostadas(long.Parse(numProtocolo));
                if (dezenasApostadas == null)
                {
                    MessageBox.Show("Protocolo " + numProtocolo + " não localizado, tente novamente");
                    protocoloNum.Clear();
                    protocoloNum.Focus();
                }
                else
                {
                    //criar list para o protocolo consultado
                    List<String> protocoloJogado = new List<String> { };//para inserir as dezenas do protocolo jogado
                    List<Int32> allTimesJogados = new List<Int32> { };//para inserir os times do protocolo jogado
                    string numerosSorteados = endPoint.ObterTodosNumerosSorteados();
                    if (numerosSorteados == "")
                    {
                        MessageBox.Show("O sorteio ainda não foi realizado");
                    }
                    else
                    {
                        if (numerosSorteados == "-1" || numerosSorteados == "-2")
                        {
                            MessageBox.Show("Sistema temporariamente fora do ar, tente mais tarde.");
                        }
                        else
                        {
                            int totalAcerto = 0, totalTimeAcerto = 0;
                            //consultar time sorteado
                            string timeSorteado = endPoint.obterNomeTimeSorteado();
                            listTime[timeToNumber(timeSorteado)].Image = Image.FromFile(@"..\..\img\" + timeToNumber(timeSorteado) + ".png");
                            //calculo para dezenas apostadas
                            int contaDezenasApostadas = dezenasApostadas.Count(f => f == ',');
                            contaDezenasApostadas = contaDezenasApostadas + 1;
                            string[] values = dezenasApostadas.Split(',');
                            for (int k = 0; k < contaDezenasApostadas; k++)
                            {
                                listButton[Int32.Parse(values[k])].BackColor = Color.Azure;
                                listButton[Int32.Parse(values[k])].FlatStyle = FlatStyle.Popup;
                                int timesApostados = changeTime(Convert.ToInt32((values[k])));
                                protocoloJogado.Add(values[k].ToString());
                                if (timesApostados == timeToNumber(timeSorteado))
                                {
                                    totalTimeAcerto++;
                                }
                            }
                            //consultar numeros sorteados
                            int contaDezenasSorteadas = numerosSorteados.Count(f => f == ',');
                            string numerosAcertados = "";
                            contaDezenasSorteadas = contaDezenasSorteadas + 1;
                            for (i = 1; i <= contaDezenasSorteadas; i++)
                            {
                                string dezenaSorteada = endPoint.ObterResultadoPorDezena(i).ToString("00");
                                if (protocoloJogado.Contains(dezenaSorteada))
                                {
                                    dezenas.Add(Int32.Parse(dezenaSorteada));
                                    listButton[Int32.Parse(dezenaSorteada)].BackColor = Color.Gold;
                                    listButton[Int32.Parse(dezenaSorteada)].FlatStyle = FlatStyle.Popup;
                                    numerosAcertados = numerosAcertados + dezenaSorteada + ",";
                                    totalAcerto++;
                                }
                            }
                            double resultadoPremiacao = premiacao(totalAcerto);
                            double premioReceber = resultadoPremiacao + (totalTimeAcerto * 5);
                            string txtResultado;
                            if (premioReceber == 0)
                            {
                                txtResultado = "Bilhete não premiado";
                            }
                            else
                            {
                                txtResultado = "Prêmio a receber\nR$ " + premioReceber.ToString("###,##0.00");
                            }
                            log.Text = "";
                            log.Text = log.Text + "MEGATIME\n--------------------\n";
                            log.Text = log.Text + "Protocolo\n" + numProtocolo + "\n\n";
                            log.Text = log.Text + "--------------------\n";
                            log.Text = log.Text + "Time sorteado\n" + timeSorteado + "\n\n";
                            log.Text = log.Text + "Qtde. de dezenas no time\n" + totalTimeAcerto + "\n\n";
                            log.Text = log.Text + "Valor \nR$ " + (totalTimeAcerto * 5).ToString("#0.00") + "\n";
                            log.Text = log.Text + "--------------------\n";
                            log.Text = log.Text + "Dezenas sorteadas\n" + numerosSorteados + "\n\n";
                            log.Text = log.Text + "Qtde. acertadas\n" + totalAcerto + "(" + numerosAcertados.TrimEnd(',', ' ') + ")\n\n";
                            log.Text = log.Text + "Valor \nR$ " + resultadoPremiacao.ToString("###,##0.00") + "\n";
                            log.Text = log.Text + "--------------------\n\n";
                            log.Text = log.Text + txtResultado;
                        }
                    }
                }
            }
            executeJogo = false;
        }
        //imprimir recibo
        private void imprimirRecibo_Click(object sender, EventArgs e)
        {
            string numProtocolo = protocoloNum.Text;
            if (numProtocolo == "")
            {
                MessageBox.Show("Digite o número do protocolo");
                protocoloNum.Focus();
            }
            else
            {
                String dezenasApostadas = endPoint.obterTodasDezenasApostadas(long.Parse(numProtocolo));
                if (dezenasApostadas == null)
                {
                    MessageBox.Show("Protocolo " + numProtocolo + " não localizado, tente novamente");
                    protocoloNum.Clear();
                    protocoloNum.Focus();
                }
                else
                {
                    executeLimpar();
                    int contaDezenasSorteadas = dezenasApostadas.Count(f => f == ',');
                    string[] separaDezenas = dezenasApostadas.Split(',');
                    executeJogo = true;
                    foreach (string word in separaDezenas)
                    {
                        startFunc(Int32.Parse(word));
                    }
                    executeJogo = false;
                    printarJogo(long.Parse(protocoloNum.Text), 2);
                }
            }
        }
        //retorna a premiacao conquistada
        private double premiacao(int acertos)
        {
            double result;
            switch (acertos)
            {
                case 3:
                    result = 2.00;
                    break;
                case 4:
                    result = 2.00;
                    break;
                case 5:
                    result = 5.00;
                    break;
                case 6:
                    result = 233.89;
                    break;
                case 7:
                    result = 23445.92;
                    break;
                case 8:
                    result = 155465.89;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        //altera time pelo numero
        private int timeToNumber(string time)
        {
            int timeNum = 0;
            switch (time)
            {
                case "Alético Mineiro":
                    timeNum = 1;
                    break;
                case "Atlético Paranaense":
                    timeNum = 2;
                    break;
                case "Bahia":
                    timeNum = 3;
                    break;
                case "Botafogo":
                    timeNum = 4;
                    break;
                case "Ceará":
                    timeNum = 5;
                    break;
                case "Corinthians":
                    timeNum = 6;
                    break;
                case "Coritiba":
                    timeNum = 7;
                    break;
                case "Cruzeiro":
                    timeNum = 8;
                    break;
                case "Flamengo":
                    timeNum = 9;
                    break;
                case "Fluminense":
                    timeNum = 10;
                    break;
                case "Fortaleza":
                    timeNum = 11;
                    break;
                case "Goiás":
                    timeNum = 12;
                    break;
                case "Grêmio":
                    timeNum = 13;
                    break;
                case "Guarani":
                    timeNum = 14;
                    break;
                case "Internacional":
                    timeNum = 15;
                    break;
                case "Náutico":
                    timeNum = 16;
                    break;
                case "Palmeiras":
                    timeNum = 17;
                    break;
                case "Paraná":
                    timeNum = 18;
                    break;
                case "Ponte Preta":
                    timeNum = 19;
                    break;
                case "Santa Cruz":
                    timeNum = 20;
                    break;
                case "Santos":
                    timeNum = 21;
                    break;
                case "São Paulo":
                    timeNum = 22;
                    break;
                case "Sport":
                    timeNum = 23;
                    break;
                case "Vasco da Gama":
                    timeNum = 24;
                    break;
                case "Vitória":
                    timeNum = 25;
                    break;
            }
            return timeNum;
        }
        //altera o numero pelo time
        private string numberToTime(int time)
        {
            string timeNome = "";
            switch (time)
            {
                case 1:
                    timeNome = "Alético Mineiro";
                    break;
                case 2:
                    timeNome = "Atlético Paranaense";
                    break;
                case 3:
                    timeNome = "Bahia";
                    break;
                case 4:
                    timeNome = "Botafogo";
                    break;
                case 5:
                    timeNome = "Ceará";
                    break;
                case 6:
                    timeNome = "Corinthians";
                    break;
                case 7:
                    timeNome = "Coritiba";
                    break;
                case 8:
                    timeNome = "Cruzeiro";
                    break;
                case 9:
                    timeNome = "Flamengo";
                    break;
                case 10:
                    timeNome = "Fluminense";
                    break;
                case 11:
                    timeNome = "Fortaleza";
                    break;
                case 12:
                    timeNome = "Goiás";
                    break;
                case 13:
                    timeNome = "Grêmio";
                    break;
                case 14:
                    timeNome = "Guarani";
                    break;
                case 15:
                    timeNome = "Internacional";
                    break;
                case 16:
                    timeNome = "Náutico";
                    break;
                case 17:
                    timeNome = "Palmeiras";
                    break;
                case 18:
                    timeNome = "Paraná";
                    break;
                case 19:
                    timeNome = "Ponte Preta";
                    break;
                case 20:
                    timeNome = "Santa Cruz";
                    break;
                case 21:
                    timeNome = "Santos";
                    break;
                case 22:
                    timeNome = "São Paulo";
                    break;
                case 23:
                    timeNome = "Sport";
                    break;
                case 24:
                    timeNome = "Vasco da Gama";
                    break;
                case 25:
                    timeNome = "Vitória";
                    break;
            }
            return timeNome;
        }
        //criar jogo aleatorio
        private void aleatorio_Click(object sender, EventArgs e)
        {
            List<int> aleatorio = new List<int>();
            Random rndDezenas = new Random();
            int result, i = 0, totalRun = dezenas.Count;
            if (totalRun == 20)
            {
                MessageBox.Show("Você já completou o limite de 20 dezenas, limpe e comece novamente");
            }
            else
            {
                do
                {
                    result = rndDezenas.Next(0, 99);
                    if (!aleatorio.Contains(result))
                    {
                        aleatorio.Add(result);
                        startFunc(result);
                        i++;
                    }
                } while (i != (20 - totalRun));
                aleatorio.Clear();
            }

        }
        //limpar a tela botao
        private void limpar_Click(object sender, EventArgs e)
        {
            executeLimpar();
        }
        //executar limpeza do jogo;
        private void executeLimpar()
        {
            while (dezenas.Count != 0)
            {
                int thisDezena = dezenas[0];
                removeNumber(thisDezena);
                checkStatus(thisDezena);
            }
            log.Text = "";
            executeJogo = true;
            toSendMail.Visible = false;
            label27.Text = ("R$ 0,00");
            for (int i = 1; i <= 25; i++)
            {
                listTime[i].Image = Image.FromFile(@"..\..\img\" + i + "off.png");
            }
        }
        //seleciona pelo time
        private void selectByTime(object sender, EventArgs e)
        {
            PictureBox teamImg = (PictureBox)sender;
            int numTeam = Int32.Parse(teamImg.Name.Substring(4, 2));
            if (dezenas.Count > 16)
            {
                if (times.Contains(numTeam))
                {
                    chooseAllDezenas(numTeam);
                }
                else
                {
                    MessageBox.Show("Você irá exceder o limite de 20 dezenas");
                }
            }
            else
            {
                chooseAllDezenas(numTeam);
            }
        }
        //escolher todas as dezenas
        private void chooseAllDezenas(int numTeam)
        {
            int maxTeam, minTeam;
            maxTeam = numTeam * 4;
            minTeam = maxTeam - 3;
            if (maxTeam == 100)
            {
                maxTeam = minTeam + 2;
                startFunc(0);
            }
            for (int i = maxTeam; i >= minTeam; i--)
            {
                startFunc(i);
            }
        }
        //abrir painel de email
        private void openMailPanel(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelMail.Visible = true;
            panelMainExecute.Visible = false;
        }
        //enviar email
        private void sendMail(object sender, EventArgs e)
        {
            button2.Image = Image.FromFile(@"..\..\img\loading.gif");
            Thread.Sleep(2000);
            string toMail;
            toMail = emailBox.Text;
            if (toMail == "")
            {
                MessageBox.Show("Digite o email para enviar");
            }
            else
            {
                if (IsValidEmail(toMail))
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("Mega Time<megatimepuc@gmail.com>");
                        mail.To.Add(toMail);
                        mail.Subject = "Comprovante de aposta";
                        mail.Body = resultApsotado;
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("megatimepuc", "Megatime2015");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                        MessageBox.Show("Email enviado com sucesso, boa sorte!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Não foi possível enviar o email, tente mais tarde");
                    }
                    button2.Image = Image.FromFile(@"..\..\img\sendemail.png");
                }
                else
                {
                    MessageBox.Show("Verifique se o email está correto");
                }
            }
        }
        //checar se o email digitado é valido
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        //login button
        private void loginButton_Click(object sender, EventArgs e)
        {
            string user, password;
            user = usuario.Text;
            password = senha.Text;
            if (user=="" && password=="")
            {
                MessageBox.Show("Digite seu usuário e senha");
            }
            else
            {
                //conectar ao banco de dados
                //IMPORTANTE O QUE ESTA COMENTADO DAQUI PARA BAIXO É A CONEXÃO QUE FOI RETIRADA
                //MySql.Data.MySqlClient.MySqlConnection conn;
                //string myConnectionString;
                //myConnectionString = "server=empresaverificada.com.br;uid=megatime;" + "pwd=Megatime2015;database=megatime;";
                //try
                //{
                    loginButton.Text = "Conectando...";
                    //conn = new MySql.Data.MySqlClient.MySqlConnection();
                    //conn.ConnectionString = myConnectionString;
                    //conn.Open();
                    //string Query = "SELECT * FROM users WHERE login=@usuario AND password=@senha";
                    //MySqlCommand query = new MySqlCommand(Query, conn);
                    //usuario senha
                    //query.Parameters.AddWithValue("@usuario", user);
                    //query.Parameters.AddWithValue("@senha", password);
                    //MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    //MySqlDataReader dr = query.ExecuteReader();
                    //if (dr.Read())
                    //{
                        //String nomeUser = dr.GetString(3);
                        //nomeLogged.Text = nomeUser;
                        //DateTime time = DateTime.Now;             // Use current time.
                        //string format = "yyyy-MM-dd HH:mm:ss";   // Use this format.
                        //string updateNow = time.ToString(format);
                        loginPanel.Visible = false;
                        logoffPanel.Visible = true;
                        mainPanel.Enabled = true;
                        usuario.Text = "";
                        senha.Text = "";
                    //}
                    //else
                    //{
                        //MessageBox.Show("Usuário e/ou senha inválido(s)");
                    //}
                    loginButton.Text = "Conectar";
                //}
                //catch (MySql.Data.MySqlClient.MySqlException)
                //{
                    //log.Text = "Banco de dados - Erro";
                //}
            }
        }
        //logoff button
        private void logoff_Click(object sender, EventArgs e)
        {
            executeLimpar();
            voltaPaineis();
            mainPanel.Enabled = false;
            logoffPanel.Visible = false;
            loginPanel.Visible = true;
        }

        private void help_Click(object sender, EventArgs e)
        {
            string txtHelp = "Regras do jogo\n\n- O apostador deve escolher entre 10 e 20 dezenas.\n- As dezenas escolhidas pelo jogador devem equivaler a, pelo menos, 5 times diferentes.\n\nValores\n\n- O preço do jogo para 10 dezenas é de R$ 5,00.\n- Adicional de R$ 0,75 para cada dezena escolhida além da 10ª até a 15ª.\n- Adicional de R$ 3,00 para cada dezena além da 15ª até a 19ª.\n- Adicional de mais 7,00 se o jogo tiver exatamente 20 dezenas.\n- Independentemente do número de dezenas apostadas, deve-se acrescentar R$ 1,25 por time apostado que exceder o mínimo de 5 times.";
            MessageBox.Show(txtHelp);
        }

        private void info_Click(object sender, EventArgs e)
        {
            string txtInfo = "Puc Campinas\nSistemas de Informação Matutino\nProjeto Integrado A\nProfessor Ivan Granja\nTema: Megatime\n\nDesenvolvedores\n\nAdriano Araújo dos Reis Botega - RA 13005335\nGiovana Moraes da Silva - RA 15158058\nMarcelo Monteiro da Silva Valente - RA 15246903\n\nFinalizado e apresentado em 16/06/2015";
            MessageBox.Show(txtInfo);
        }
    }
}
