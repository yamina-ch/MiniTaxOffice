<script setup>

// ======================================================
// 1. IMPORTAMOS "ref"
// ======================================================

/*
  ref viene de Vue.

  Nos permite crear una variable reactiva.

  "Reactiva" significa que cuando cambia su valor,
  Vue actualiza automáticamente la pantalla.
*/
import { ref } from 'vue'


// ======================================================
// 2. PÁGINA ACTIVA
// ======================================================

/*
  Creamos la variable activePage.

  Al principio contiene "inicio".

  Esto significa que cuando abrimos la aplicación,
  el botón Inicio aparecerá seleccionado.
*/
const activePage = ref('inicio')


// ======================================================
// 3. COMUNICACIÓN CON APP.VUE
// ======================================================

/*
  Sidebar.vue es un componente HIJO de App.vue.

  Queremos que Sidebar pueda avisar a App.vue:

  "El usuario ha pulsado Inicio"

  o:

  "El usuario ha pulsado Ciudadanos"


  Para eso Vue utiliza "emit".
*/
const emit = defineEmits(['cambiarPagina'])


// ======================================================
// 4. FUNCIÓN PARA CAMBIAR DE PÁGINA
// ======================================================

/*
  Esta función recibe una página.

  Ejemplos:

  cambiarPagina('inicio')

  cambiarPagina('ciudadanos')
*/
function cambiarPagina(pagina) {

  /*
    PASO 1:

    Guardamos qué página está seleccionada.

    Si pagina = "ciudadanos":

    activePage.value = "ciudadanos"
  */
  activePage.value = pagina


  /*
    PASO 2:

    Avisamos a App.vue.

    Enviamos dos cosas:

    1. Nombre del evento:
       "cambiarPagina"

    2. Información:
       pagina

    Ejemplo:

    emit('cambiarPagina', 'ciudadanos')
  */
  emit('cambiarPagina', pagina)
}

</script>


<template>

  <!-- ==================================================
       BARRA LATERAL
       ================================================== -->

  <aside class="sidebar">


    <!-- =================================================
         LOGO
         ================================================= -->

    <div class="brand">

      <!-- Icono -->
      <div class="brand-icon">
        🏛️
      </div>


      <!-- Nombre de la aplicación -->
      <div>

        <h1>
          MiniTaxOffice
        </h1>

        <span>
          Oficina Tributaria
        </span>

      </div>

    </div>



    <!-- =================================================
         MENÚ DE NAVEGACIÓN
         ================================================= -->

    <nav class="navigation">


      <!-- =================================================
           BOTÓN INICIO
           ================================================= -->

      <a
        href="#"

        class="nav-item"

        :class="{ active: activePage === 'inicio' }"

        @click.prevent="cambiarPagina('inicio')"
      >

        <!-- Icono -->
        <span>⌂</span>

        <!-- Nombre -->
        <span>Inicio</span>

      </a>



      <!-- =================================================
           BOTÓN CIUDADANOS
           ================================================= -->

      <a
        href="#"

        class="nav-item"

        :class="{ active: activePage === 'ciudadanos' }"

        @click.prevent="cambiarPagina('ciudadanos')"
      >

        <!-- Icono -->
        <span>👤</span>

        <!-- Nombre -->
        <span>Ciudadanos</span>

      </a>


    </nav>

  </aside>

</template>


<style scoped>

/*
  "scoped" significa que este CSS
  solamente afecta a Sidebar.vue.
*/


/* ======================================================
   BARRA LATERAL
   ====================================================== */

.sidebar {

  /*
    Ancho de nuestra barra lateral.
  */
  width: 260px;


  /*
    100vh = toda la altura de la ventana.
  */
  min-height: 100vh;


  /*
    Color azul oscuro del Sidebar.
  */
  background-color: #111f3a;


  /*
    Color del texto.
  */
  color: white;


  /*
    Espacio interior.
  */
  padding: 30px 20px;


  /*
    Hace que el padding esté incluido
    dentro de los 260px de ancho.
  */
  box-sizing: border-box;
}



/* ======================================================
   LOGO / BRAND
   ====================================================== */

.brand {

  /*
    Colocamos:

    🏛️ MiniTaxOffice

    en horizontal.
  */
  display: flex;


  /*
    Centramos verticalmente.
  */
  align-items: center;


  /*
    Espacio entre el icono
    y el texto.
  */
  gap: 12px;
}



/* ======================================================
   ICONO DEL LOGO
   ====================================================== */

.brand-icon {

  /*
    Fondo blanco.
  */
  background-color: white;


  /*
    Espacio interior.
  */
  padding: 12px;


  /*
    Esquinas redondeadas.
  */
  border-radius: 12px;
}



/* ======================================================
   TÍTULO
   ====================================================== */

.brand h1 {

  /*
    Tamaño de MiniTaxOffice.
  */
  font-size: 20px;


  /*
    Los h1 tienen margen automáticamente.

    Lo quitamos.
  */
  margin: 0;
}



/* ======================================================
   SUBTÍTULO
   ====================================================== */

.brand span {

  /*
    Texto más pequeño.
  */
  font-size: 13px;


  /*
    Gris claro.
  */
  color: #aab6cc;
}



/* ======================================================
   MENÚ
   ====================================================== */

.navigation {

  /*
    Separamos el menú del logo.
  */
  margin-top: 40px;


  /*
    Utilizamos Flexbox.
  */
  display: flex;


  /*
    Los elementos aparecen verticalmente:

    Inicio
    Ciudadanos

    y no:

    Inicio Ciudadanos
  */
  flex-direction: column;


  /*
    Espacio entre los botones.
  */
  gap: 8px;
}



/* ======================================================
   BOTÓN NORMAL
   ====================================================== */

.nav-item {

  /*
    Icono y texto en horizontal:

    👤 Ciudadanos
  */
  display: flex;


  /*
    Centramos verticalmente
    el icono y el texto.
  */
  align-items: center;


  /*
    Espacio entre icono y texto.
  */
  gap: 15px;


  /*
    Espacio dentro del botón.
  */
  padding: 15px;


  /*
    Texto blanco.
  */
  color: white;


  /*
    Quitamos el subrayado
    que tienen los enlaces <a>.
  */
  text-decoration: none;


  /*
    Esquinas redondeadas.
  */
  border-radius: 10px;


  /*
    Normalmente el botón
    no tiene fondo.
  */
  background-color: transparent;


  /*
    Hace que el cambio de color
    sea más suave.
  */
  transition: background-color 0.2s;
}



/* ======================================================
   BOTÓN SELECCIONADO
   ====================================================== */

/*
  Esta regla se aplica cuando tenemos:

  class="nav-item active"

  La clase active la controla Vue mediante:

  :class="{ active: condición }"
*/
.nav-item.active {

  /*
    Azul del botón seleccionado.
  */
  background-color: #2563eb;
}



/* ======================================================
   HOVER
   ====================================================== */

/*
  :hover significa:

  "cuando ponemos el ratón encima".


  :not(.active) significa:

  "pero solamente si NO está seleccionado".
*/
.nav-item:not(.active):hover {

  background-color: #1e3a5f;
}

</style>