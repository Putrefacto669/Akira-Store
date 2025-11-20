using System;
using System.Collections.Generic;

public class Vinilo
{
    public int IdVinilo { get; set; }
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public string Genero { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Imagen { get; set; } // Archivo
}

public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
}

public class Venta
{
    public int IdVenta { get; set; }
    public int IdCliente { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }

    // Relación
    public Cliente Cliente { get; set; }
    public List<DetalleVenta> Detalles { get; set; }
}

public class DetalleVenta
{
    public int IdDetalle { get; set; }
    public int IdVenta { get; set; }
    public int IdVinilo { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }

    // Relaciones
    public Vinilo Vinilo { get; set; }
}
