﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ThreadNetwork;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Repositories;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class ActividadesService
    {
        HttpClient cliente;

        Repositories.ActividadesRepository actividadesRepository = new();
        public ActividadesService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("http://localhost:55555/")
            };
        }

        public event Action? DatosActualizadosAct;

        public async Task GetActividades()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);

                bool aviso = false;

                var response = await cliente.GetFromJsonAsync<List<ActividadDTO>>($"/Actividades/{fecha:yyyy-MM-dd}/{fecha:HH}/{fecha:mm}");
                if (response != null)
                {
                    foreach (ActividadDTO actividad in response)
                    {
                        var entidad = actividadesRepository.Get(actividad.Id);

                        //estado 0 = Borrador, estado 1 = Publicado, estado 2 = eliminado
                        if (entidad == null && actividad.Estado == 3) // 2 si esta eliminado
                        {
                            entidad = new()
                            {
                                Id = actividad.Id,
                                Titulo = actividad.Titulo,
                                Descripcion = actividad.Descripcion,
                                FechaRealizacion = actividad.FechaRealizacion,
                            };
                            actividadesRepository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                if (actividad.Estado == 2)
                                {
                                    actividadesRepository.Delete(entidad);
                                    aviso = true;
                                }
                                else
                                {

                                    if (actividad.Titulo != entidad.Titulo || actividad.Descripcion != actividad.Descripcion
                                        || actividad.FechaRealizacion != actividad.FechaRealizacion)
                                    {
                                        actividadesRepository.Update(entidad);
                                        aviso = true;
                                    }
                                }
                            }
                        }


                    }

                    if (aviso)
                    {

                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            DatosActualizadosAct?.Invoke();
                        });
                    }

                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.FechaActualizacion));
                }
            }
            catch
            {

            }
        }

        public async Task<IEnumerable<ActividadDTO>?>? GetAll(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.GetAsync("/Actividades");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<ActividadDTO>>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            return null;
        }


        public async Task<ActividadDTO?> Get(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.GetAsync($"/Actividad/{dto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ActividadDTO>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            return null;
        }
        public async Task Insert(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.PostAsJsonAsync("/AgregarAct", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        public async Task Update(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.PutAsJsonAsync("/EditarAct", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        public async Task Delete(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.DeleteAsync($"/EliminarAct/{dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
