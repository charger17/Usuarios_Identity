namespace Usuarios_identity.Utilidades
{
    public class funcionesAdicionales
    {
        public int CalculaEdad(DateTime edad)
        {
            return (int)DateTime.Today.AddTicks(-edad.Ticks).Year - 1;
        }
    }
}
