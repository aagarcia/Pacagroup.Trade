﻿using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.IdentityModel.Tokens;
using Pacagroup.Trade.Application.UseCases.Commons.Exceptions;
using Pacagroup.Trade.Services.gRPC.Protos;

namespace Pacagroup.Trade.Services.gRPC.Commons.GlobalException
{
    public class GlobalExceptionHandler : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, 
                                                                                      ServerCallContext context, 
                                                                                      UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation);
            }
            catch (ValidationExceptionCustom ex)
            {
                var serverResponse = new ServerResponse()
                {
                    IsSuccess = false,
                    Message = "Errores de Validación",
                    Errors = string.Join(" | ", ex.Errors)
                };

                return MapResponse<TRequest, TResponse>(serverResponse);
            }
            catch (Exception ex)
            {
                var serverResponse = new ServerResponse()
                {
                    IsSuccess = false,
                    Message = "Errores durante la ejecución de la operación",
                    Errors = ex.Message
                };

                return MapResponse<TRequest, TResponse>(serverResponse);
            }
        }

        private TResponse MapResponse<TRequest, TResponse>(ServerResponse serverResponse)
        {
            var response = Activator.CreateInstance<TResponse>();
            SetNestedPropertyValue(response, "ServerResponse.IsSuccess", serverResponse.IsSuccess);
            SetNestedPropertyValue(response, "ServerResponse.Message", serverResponse.Message);
            SetNestedPropertyValue(response, "ServerResponse.Errors", serverResponse.Errors);

            return response;
        }

        private static void SetNestedPropertyValue<T>(T obj, string propertyPath, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyPath))
            {
                throw new ArgumentNullException(nameof(obj), "El objeto o la ruta de la propiedad no pueden ser nulos.");
            }

            var properties = propertyPath.Split('.');
            var currentObject = (object)obj;

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyName = properties[i];
                var property = currentObject.GetType().GetProperty(propertyName);

                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on type '{currentObject.GetType().FullName}'");
                }

                if (i == properties.Length - 1)
                {
                    property.SetValue(currentObject, value);
                }
                else
                {
                    var nextObject = property.GetValue(currentObject);

                    if (nextObject == null)
                    {
                        nextObject = Activator.CreateInstance(property.PropertyType);
                        property.SetValue(currentObject, nextObject);
                    }

                    currentObject = nextObject;
                }
            }
        }
    }
}
