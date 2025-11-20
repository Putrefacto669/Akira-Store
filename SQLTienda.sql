-- Crear base de datos (solo si aún no existe)
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name='TiendaVinilos')
    CREATE DATABASE TiendaVinilos;
GO

USE TiendaVinilos;
GO

------------------------------------------------------
-- 🟦 TABLA: Vinilos
------------------------------------------------------
IF OBJECT_ID('Vinilos', 'U') IS NOT NULL DROP TABLE Vinilos;
GO

CREATE TABLE Vinilos (
    IdVinilo INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(200) NOT NULL,
    Artista NVARCHAR(200) NOT NULL,
    Genero NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Imagen NVARCHAR(200)
);

------------------------------------------------------
-- 🟩 TABLA: Clientes
------------------------------------------------------
IF OBJECT_ID('Clientes', 'U') IS NOT NULL DROP TABLE Clientes;
GO

CREATE TABLE Clientes (
    IdCliente INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(200) NOT NULL,
    Correo NVARCHAR(200),
    Telefono NVARCHAR(50)
);

------------------------------------------------------
-- 🟧 TABLA: Ventas
------------------------------------------------------
IF OBJECT_ID('Ventas', 'U') IS NOT NULL DROP TABLE Ventas;
GO

CREATE TABLE Ventas (
    IdVenta INT PRIMARY KEY IDENTITY,
    IdCliente INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
);

------------------------------------------------------
-- 🟥 TABLA: DetalleVenta
------------------------------------------------------
IF OBJECT_ID('DetalleVenta', 'U') IS NOT NULL DROP TABLE DetalleVenta;
GO

CREATE TABLE DetalleVenta (
    IdDetalle INT PRIMARY KEY IDENTITY(1,1),
    IdVenta INT NOT NULL,
    IdVinilo INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta),
    FOREIGN KEY (IdVinilo) REFERENCES Vinilos(IdVinilo)
);

------------------------------------------------------
-- 🟨 DATOS DE PRUEBA
------------------------------------------------------

INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Dirt', 'Alice In Chains', 'Grunge', 20.99, 10, 'album1.jpg'),
('White Pony', 'Deftones', 'Metal Alt', 18.99, 10, 'album2.jpg'),
('Bocanada', 'Gustavo Cerati', 'Electro', 25.99, 10, 'album3.jpg'),
('Cancion Animal', 'Soda Stereo', 'Rock', 25.99, 10, 'album5.jpg'),
('Roots', 'Sepultura', 'Nu Metal', 25.99, 10, 'album6.jpg'),
('Ten', 'Pearl Jam', 'Grunge', 25.99, 10, 'album7.jpg'),
('Vulgar Display Of Power', 'Pantera', 'Groove Metal', 25.99, 10, 'album10.jpg'),
('Life Is Peachy', 'Korn', 'Nu Metal', 25.99, 10, 'album11.jpg'),
('Fallen', 'Evanescense', 'Nu Metal', 25.99, 10, 'album12.jpg'),
('Thriller', 'Michael Jackson', 'Pop', 25.99, 10, 'album13.jpg'),
('Hybrid Theory', 'Linkin Park', 'Nu Metal', 25.99, 10, 'album16.jpg'),
('Demon Days', 'Gorillaz', 'Alt', 25.99, 10, 'album17.jpg'),
('OK COMPUTER', 'Radiohead', 'Alt', 25.99, 10, 'album18.jpg'),
('Facelift', 'Alice In Chains', 'Grunge', 25.99, 10, 'album9.jpg'),
('The Dark Side Of The Moon', 'Pink Floyd', 'Rock', 25.99, 10, 'album15.jpg');

INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Shed', 'Title Fight', 'Metal Alt', 25.99, 10, 'album21.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Purple', 'Stone Temple Pilots', 'Grunge', 25.99, 10, 'album23.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('jar', 'SuperHeaven', 'Shoegaze', 25.99, 10, 'album22.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Electric Heart', 'Marina And The Diamonds', 'Alt', 25.99, 10, 'album20.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Let Go', 'Avril Lavigne', 'Alt', 25.99, 10, 'album19.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Random Access Memories', 'Daft Punk', 'Electro', 25.99, 10, 'album14.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Ride The Lightning', 'Metallica', 'Trash Metal', 25.99, 10, 'album8.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('In Utero', 'Nirvana', 'Grunge', 25.99, 10, 'album4.jfif');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Meteora', 'Linkin Park', 'Nu Metal', 25.99, 10, 'album24.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Korn', 'Korn', 'Nu Metal', 25.99, 10, 'album25.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Paranoid', 'Black Sabbath', 'Metal', 25.99, 10, 'album26.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('IOWA', 'Slipknot', 'Nu Metal', 25.99, 10, 'album27.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('BadMotorFinger', 'Soundgarden', 'Grunge', 25.99, 10, 'album28.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Artaud', 'Pescado Rabioso', 'Jazz-Rock', 25.99, 10, 'album29.jpg');
INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) VALUES
('Amor Amarillo', 'Gustavo Cerati', 'Rock', 25.99, 10, 'album30.jpg');











-- Eliminar tablas en orden correcto (por dependencias de FK)
DROP TABLE IF EXISTS DetalleVenta;
DROP TABLE IF EXISTS Ventas;

-- Crear tabla Ventas
CREATE TABLE Ventas (
    IdVenta INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME NOT NULL,
    Total DECIMAL(10,2) NOT NULL
);

-- Crear tabla DetalleVenta 
