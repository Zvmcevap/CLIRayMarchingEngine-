using System.Numerics;
using CLIRayMarchingEngine;

class Program
{
    static void Main(string[] args)
    {

        AppManager gameManager = AppManager.Instance;
        gameManager.Init(200, 60, 0.5f, new(0f, 0f, -7f));
        gameManager.Run();
    }

    #region Chaos Game Objects
    public static Chaos GetSierpinski(AppManager gameManager)
    {
        return new Chaos(
                0.5f,
                new Vector3(gameManager.Width / 2f, 1f, 0f),
                new Vector3(2f, gameManager.Height - 2f, 0f),
                new Vector3(gameManager.Width - 2f, gameManager.Height - 2f, 0f)
                );
    }

    public static Chaos GetKvadratRaj(AppManager gameManager)
    {
        return new Chaos(
                0.5f,
                new Vector3(2f, 2f, 0f),
                new Vector3(gameManager.Width - 2f, 2f, 0f),
                new Vector3(2f, gameManager.Height - 2f, 0f),
                new Vector3(gameManager.Width - 2f, gameManager.Height - 2f, 0f)
                );
    }
    public static Chaos GetAlPaPetkot(AppManager gameManager)
    {
        return new Chaos(
                0.55f,
                new Vector3(gameManager.Width / 2f, 2f, 0f),
                new Vector3(20f, 25f, 0f),
                new Vector3(gameManager.Width - 20f, 25f, 0f),
                new Vector3(40f, gameManager.Height - 2f, 0f),
                new Vector3(gameManager.Width - 40f, gameManager.Height - 2f, 0f)
                );
    }
    #endregion
}
