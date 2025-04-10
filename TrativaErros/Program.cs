using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

class Time
{
    public string Nome { get; set; }
    public int Pontos { get; set; }

    public Time(string nome)
    {
        Nome = nome;
        Pontos = 0;
    }

    public void AdicionarPontos(int pontos) => Pontos += pontos;
}

class Partida
{
    public Time Time1 { get; set; }
    public Time Time2 { get; set; }
    public int GolsTime1 { get; set; }
    public int GolsTime2 { get; set; }

    public Partida(Time time1, Time time2, int golsTime1, int golsTime2)
    {
        Time1 = time1;
        Time2 = time2;
        GolsTime1 = golsTime1;
        GolsTime2 = golsTime2;
    }

    public void RegistrarResultado()
    {
        if (GolsTime1 > GolsTime2)
            Time1.AdicionarPontos(3);
        else if (GolsTime2 > GolsTime1)
            Time2.AdicionarPontos(3);
        else
        {
            Time1.AdicionarPontos(1);
            Time2.AdicionarPontos(1);
        }
    }
}

class Torneio
{
    private List<Time> times = new List<Time>();
    private List<Partida> partidas = new List<Partida>();
    private static readonly string logFilePath = "log.txt";

    public void AdicionarTime(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || times.Any(t => t.Nome == nome))
        {
            LogErro($"Erro: Nome inválido ou time já existe");
            Console.WriteLine($"❌ Erro: Nome inválido ou time já existe");
            return;
        }

        times.Add(new Time(nome));
        Console.WriteLine($"✅ Time \"{nome}\" adicionado com sucesso!");
    }

    public void CriarPartida(string nomeTime1, string nomeTime2, int golsTime1, int golsTime2)
    {
        var time1 = times.FirstOrDefault(t => t.Nome == nomeTime1);
        var time2 = times.FirstOrDefault(t => t.Nome == nomeTime2);

        if (time1 == null || time2 == null || golsTime1 < 0 || golsTime2 < 0)
        {
            LogErro("Erro: Time(s) não existe(m) ou número inválido de gols");
            Console.WriteLine("❌ Erro: Time(s) não existe(m) ou número inválido de gols");
            return;
        }

        var partida = new Partida(time1, time2, golsTime1, golsTime2);
        partida.RegistrarResultado();
        partidas.Add(partida);

        Console.WriteLine($"✅ Partida entre \"{time1.Nome}\" e \"{time2.Nome}\" criada com sucesso!");
    }

    public void ImprimirResultados()
    {
        Console.WriteLine("\nResultados:");
        foreach (var partida in partidas)
        {
            Console.WriteLine($"{partida.Time1.Nome} {partida.GolsTime1} x {partida.GolsTime2} {partida.Time2.Nome}");
        }
    }

    public void ImprimirClassificacao()
    {
        Console.WriteLine("\nClassificação Final:");
        var timesOrdenados = times.OrderByDescending(t => t.Pontos).ToList();
        for (int i = 0; i < timesOrdenados.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {timesOrdenados[i].Nome} ({timesOrdenados[i].Pontos} pontos)");
        }
    }

    private void LogErro(string mensagem)
    {
        try
        {
            using (var sw = new StreamWriter(logFilePath, true, Encoding.UTF8))
            {
                sw.WriteLine($"[{DateTime.Now}] {mensagem}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao registrar log: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Torneio torneio = new Torneio();

        torneio.AdicionarTime("Brasil");
        torneio.AdicionarTime("");
        torneio.AdicionarTime("Canadá");
        torneio.AdicionarTime("Argentina");
        torneio.AdicionarTime("Angola");

        torneio.CriarPartida("Brasil", "Canadá", 1, 0);
        torneio.CriarPartida("Argentina", "Angola", 2, 0);
        torneio.CriarPartida("Brasil", "Argentina", -10, -2);
        torneio.CriarPartida("Brasil", "Argentina", 0, 2);
        torneio.CriarPartida("Angola", "Canadá", 1, 1);
        torneio.CriarPartida("Brasil", "Angola", 3, 2);
        torneio.CriarPartida("Argentina", "Nigéria", 3, 3);
        torneio.CriarPartida("Argentina", "Canadá", 2, 4);

        torneio.ImprimirClassificacao();
        torneio.ImprimirResultados();
    }
}
