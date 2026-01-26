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
            // 1. Crear Entidad (Como antes)
            var nuevoPedido = new Pedido
            {
                NombreCliente = dto.NombreCliente,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Fecha = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Detalles = new List<DetallePedido>()
            };

            decimal totalCalculado = 0;

            // 2. Procesar Ítems
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

            await _unitOfWork.Pedidos.AddAsync(nuevoPedido);
            await _unitOfWork.CompleteAsync();

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
    }
}