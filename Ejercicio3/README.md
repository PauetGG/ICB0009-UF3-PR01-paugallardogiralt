# 🚗 Pregunta teórica 1 - Control del paso por el túnel

## 📚 Explicación de ventajas e inconvenientes

### ✅ Controlar el paso en el servidor

- El servidor garantiza que solo un vehículo cruce el túnel a la vez, evitando colisiones.
- Toda la lógica de control está centralizada, facilitando el mantenimiento y asegurando la coherencia entre todos los clientes.
- Evitamos errores de sincronización que podrían ocurrir si cada cliente decide por su cuenta.

**Inconvenientes:**
- El servidor asume más carga de trabajo.
- Existe una pequeña latencia, ya que los clientes deben esperar confirmaciones para poder seguir avanzando.

---

### ✅ Controlar el paso en el cliente

- Menor carga de trabajo para el servidor.
- El cliente puede reaccionar de manera inmediata, sin esperar respuesta.

**Inconvenientes:**
- Mayor riesgo de inconsistencias: varios vehículos podrían creer que pueden cruzar simultáneamente.
- La lógica de control sería más complicada de implementar en cada cliente.

---

# 🚗 Pregunta teórica 2 - Gestión de colas de espera y priorización por dirección

## 📚 Explicación de la gestión de colas

- El servidor debe llevar la cuenta de qué vehículo está cruzando el puente (`vehiculoEnPuente`).
- Además, haría **dos colas distintas**:
  - `colaNorte` para los que quieren ir al norte.
  - `colaSur` para los que quieren ir al sur.

Cuando el coche que está cruzando termina, el servidor mira las colas y deja pasar al siguiente que toque.

---

## 🛠️ Estructura de datos elegida

- Utilizaría dos colas (`Queue`), una para cada dirección.
- Así, mantenemos el orden de llegada y cada vehículo pasa cuando le toca.

---

## 🎯 Justificación de la elección

- **Justicia:** quien llega antes, pasa antes.
- **Control de tráfico:** podríamos hacer reglas para dejar pasar 2 o 3 del norte, y luego 2 o 3 del sur, evitando que una dirección acapare todo el tiempo.
- **Fácil de programar:** las colas (`Queue`) son estructuras simples y muy eficientes para este tipo de casos.

---

## 🚦 Visualización en los clientes

Cada coche debería poder ver su estado en todo momento:

- `"Esperando (norte)"` o `"Esperando (sur)"` ➔ si está esperando su turno en la cola.
- `"Cruzando puente"` ➔ si le ha tocado y está pasando el túnel.
- `"Circulando"` ➔ si simplemente avanza por la carretera fuera del puente.


