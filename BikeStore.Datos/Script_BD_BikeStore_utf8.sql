Create Database BD_BikeStore;
Go

Use BD_BikeStore;
Go

--Tabla Categoria
Create Table Categoria (
	IdCategoria Int Identity(1,1) Primary Key,
	Nombre NVarchar(50) Not Null,
	Descripcion NVarchar(200),
	Activo Bit Default 1
);
Go

--tabla Bicicleta
Create Table Bicicleta (
	IdBicicleta Int Identity(1,1) Primary Key,
	IdCategoria Int Not Null,
	Marca NVarchar(50) Not Null,
	Modelo NVarchar(50) Not Null,
	Precio Decimal(10,2) Not Null,
	Stock Int Not Null,
	Estado NVarchar(20) Not Null,
	Constraint FK_Bicicleta_Categoria Foreign Key (IdCategoria) References Categoria(IdCategoria)

);
Go

--Tabla Cliente
Create Table Cliente(
	IdCliente Int Identity(1,1) Primary Key,
	Cedula NVarchar(20) Not Null Unique,
	Nombres NVarchar(100) NoT Null,
	Apellidos NVarchar(100) Not Null,
	Telefono NVarchar(20),
	Correo NVarchar(100)
);
Go

--Tabla Venta Depende de cleinte
Create Table Venta (
	IdVenta Int Identity(1,1) Primary Key,
	Fecha DateTime Not Null Default GetDate(),
	IdCliente Int Not Null,
	Total Decimal(10,2) Not Null,
	Constraint FK_Venta_Cliente Foreign Key (IdCliente) References Cliente(IdCliente)
);
Go

--Tabla Detalle venta
Create Table Detalle_Venta (
	IdDetalle Int Identity(1,1) Primary Key,
	IdVenta Int Not Null,
	IdBicicleta Int Not Null,
	Cantidad Int Not Null,
	Precio Decimal(10,2) Not Null,
	SubTotal Decimal(10,2) Not Null,
	Constraint FK_DetalleVenta_Venta Foreign Key (IdVenta) References Venta(IdVenta),
	Constraint FK_DetalleVenta_Bicicleta Foreign Key (IdBicicleta) References Bicicleta(IdBicicleta)
);
Go

--Insertar Categorías
INSERT INTO Categoria (Nombre, Descripcion, Activo) VALUES
('Montaña', 'Bicicletas todoterreno con suspensión', 1),
('Ruta', 'Bicicletas ligeras para asfalto y velocidad', 1),
('BMX', 'Bicicletas para acrobacias y saltos', 1),
('Eléctricas', 'Bicicletas con pedaleo asistido por motor', 1),
('Infantiles', 'Bicicletas para niños', 1);
GO

-- Insertar Bicicletas de prueba
INSERT INTO Bicicleta (IdCategoria, Marca, Modelo, Precio, Stock, Estado) VALUES
(1, 'Trek', 'Marlin 5', 550.00, 10, 'Disponible'),
(2, 'Specialized', 'Allez', 900.00, 5, 'Disponible'),
(1, 'Giant', 'Talon 3', 600.00, 2, 'Stock Bajo'),
(4, 'Cube', 'Reaction Hybrid', 2500.00, 0, 'Agotado');
GO

-- Insertar Clientes de prueba
INSERT INTO Cliente (Cedula, Nombres, Apellidos, Telefono, Correo) VALUES
('1712345678', 'Lizandro', 'Cedeño', '0991234567', 'lizandro.cedeño@email.com'),
('0923456789', 'María', 'Zambrano', '0987654321', 'maria.zambrano@email.com');
GO
