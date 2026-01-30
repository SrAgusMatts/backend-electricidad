using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoResponseDto> CrearPedido(PedidoCreateDto dto)
        {
            var nuevoPedido = new Pedido
            {
                NombreCliente = dto.NombreCliente,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Fecha = DateTime.UtcNow,
                Estado = EstadoPedido.Pendiente,
                Detalles = new List<DetallePedido>()
            };

            decimal totalCalculado = 0;

            foreach (var itemDto in dto.Items)
            {
                var productoReal = await _unitOfWork.Productos.GetByIdAsync(itemDto.ProductoId);

                if (productoReal != null)
                {
                    var detalle = new DetallePedido
                    {
                        ProductoId = productoReal.Id,
                        NombreProducto = productoReal.Nombre,
                        Cantidad = itemDto.Cantidad,
                        PrecioUnitario = productoReal.Precio
                    };

                    totalCalculado += (detalle.PrecioUnitario * detalle.Cantidad);
                    nuevoPedido.Detalles.Add(detalle);
                }
            }

            nuevoPedido.Total = totalCalculado;

            try
            {
                await _unitOfWork.Pedidos.AddAsync(nuevoPedido);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine("🔴 ERROR CRÍTICO AL GUARDAR PEDIDO: " + mensajeError);

                throw new Exception("Error de Base de Datos: " + mensajeError);
            }

            var response = new PedidoResponseDto
            {
                Id = nuevoPedido.Id,
                Fecha = nuevoPedido.Fecha,
                Estado = nuevoPedido.Estado.ToString(),
                NombreCliente = nuevoPedido.NombreCliente,
                Total = nuevoPedido.Total,
                Items = nuevoPedido.Detalles.Select(d => new DetallePedidoResponseDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            return response;
        }
        public async Task<List<PedidoResponseDto>> ObtenerPedidos()
        {
            // Buscamos todos los pedidos e incluimos los detalles
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync(includeProperties: "Detalles");

            var response = pedidos.Select(p => new PedidoResponseDto
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Estado = p.Estado.ToString(),
                NombreCliente = p.NombreCliente,
                Telefono = p.Telefono,
                Email = p.Email,
                Total = p.Total,
                Items = p.Detalles.Select(d => new DetallePedidoResponseDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            })
            .OrderBy(x => x.Fecha)
            .ToList();

            return response;
        }
        public async Task<PedidoResponseDto?> ObtenerPedidoPorId(int id)
        {

            var todosLosPedidos = await _unitOfWork.Pedidos.GetAllAsync(includeProperties: "Detalles");

            var pedido = todosLosPedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null) return null;

            return new PedidoResponseDto
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                Estado = pedido.Estado.ToString(),
                NombreCliente = pedido.NombreCliente,
                Telefono = pedido.Telefono,
                Email = pedido.Email,
                Total = pedido.Total,
                Items = pedido.Detalles.Select(d => new DetallePedidoResponseDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }

        public async Task<bool> CambiarEstado(int id, string nuevoEstado)
        {
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync(includeProperties: "Detalles");
            var pedido = pedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null) return false;

            if (!Enum.TryParse<EstadoPedido>(nuevoEstado, true, out var estadoEnum))
            {
                return false;
            }

            if (estadoEnum == EstadoPedido.Completado && pedido.Estado != EstadoPedido.Completado)
            {
                if (pedido.Detalles != null && pedido.Detalles.Any())
                {
                    foreach (var detalle in pedido.Detalles)
                    {
                        var producto = await _unitOfWork.Productos.GetByIdAsync(detalle.ProductoId);

                        if (producto != null)
                        {
                            producto.Stock -= detalle.Cantidad;

                            if (producto.Stock < 0) producto.Stock = 0;

                            _unitOfWork.Productos.Update(producto);
                        }
                    }
                }
            }

            pedido.Estado = estadoEnum;
            _unitOfWork.Pedidos.Update(pedido);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}