# Prueba Técnica - Full Stack Developer (.NET + React)

¡Bienvenidos al repositorio de mi prueba técnica para Zoco! Este proyecto es una solución Full Stack diseñada para gestionar perfiles de usuarios, direcciones y estudios académicos a través de un panel de control jerárquico.

🔗 **[Visitar el Despliegue en Vivo del Frontend aquí (Vercel)](https://zoco-fullstack.vercel.app/)**

---

## 📂 Estructura del Proyecto

Este repositorio contiene el código fuente tanto de la API en el servidor, como la aplicación cliente en React. He separado el código en dos carpetas dentro de la raíz principal para una revisión más limpia.

### 1. [backend/](./backend/README.md)
Es la API RESTful desarrollada con **.NET 6+ / C#** Entity Framework Core sobre SQL Server. Implementa arquitectura limpia, autenticación JWT, un modelo de roles (`User`/`Admin`) y un Seeder por defecto.
👉 **Por favor leer el `README.md` de la carpeta [backend/](./backend) para conocer cómo conectar su SQL y ejecutar el proyecto localmente.**

### 2. [zoco-frontend/](./zoco-frontend/README.md)
Es la Single Page Application (SPA) cliente desarrollada con **React 18** y **Vite**. Posee un estilizado global responsivo con **Tailwind CSS v4** y cuenta con protección de rutas.
👉 **La documentación completa del lado cliente se halla en el `README.md` de la carpeta [zoco-frontend/](./zoco-frontend).**

---

## 🗝️ Credenciales de Administrador (Para pruebas)

Tanto si corres el backend en tu entorno local (al ejecutarse el Entity Framework `DbSeeder`), o si entras directo a mi Frontend de arriba y me mandas peticiones, puedes usar esta cuenta para evaluar el rol `Admin` y la vista de panel de operaciones:

- **Email:** `admin@zoco.local`
- **Password:** `Admin123!`

---
*Desarrollado con dedicación para evaluación técnica. ¡Gracias por revisar mi código!*