<script setup>

// ======================================================
// 1. IMPORTAMOS "ref"
// ======================================================

/*
  Necesitamos una variable reactiva para guardar
  qué página queremos mostrar en la zona derecha.
*/
import { ref } from 'vue'


// ======================================================
// 2. IMPORTAMOS NUESTROS COMPONENTES
// ======================================================

// Menú lateral
import Sidebar from './components/Sidebar.vue'

// Página de Inicio
import HomeView from './views/HomeView.vue'

// Página de Ciudadanos
import CitizensView from './views/CitizensView.vue'


// ======================================================
// 3. PÁGINA ACTUAL
// ======================================================

/*
  Esta variable pertenece a App.vue.

  Al abrir la aplicación queremos mostrar Inicio.

  Por eso empieza con:

  currentPage = "inicio"
*/
const currentPage = ref('inicio')


// ======================================================
// 4. FUNCIÓN PARA RECIBIR EL CAMBIO DEL SIDEBAR
// ======================================================

/*
  Sidebar.vue nos enviará:

  "inicio"

  o

  "ciudadanos"


  Esta función recibe ese valor
  y lo guarda en currentPage.
*/
function cambiarPagina(pagina) {

  currentPage.value = pagina

}

</script>


<template>

  <!-- ==================================================
       ESTRUCTURA PRINCIPAL
       ================================================== -->

  <div class="app-layout">


    <!-- =================================================
         SIDEBAR
         ================================================= -->

    <!--
      Sidebar puede emitir este evento:

      cambiarPagina


      Cuando ocurra:

      @cambiarPagina


      ejecutamos nuestra función:

      cambiarPagina
    -->
    <Sidebar @cambiarPagina="cambiarPagina" />


    <!-- =================================================
         CONTENIDO DE LA DERECHA
         ================================================= -->

    <main class="main-content">


      <!-- ===============================================
           PÁGINA INICIO
           =============================================== -->

      <!--
        v-if significa:

        mostrar este componente SOLAMENTE SI
        currentPage es igual a "inicio".
      -->
      <HomeView
        v-if="currentPage === 'inicio'"
      />


      <!-- ===============================================
           PÁGINA CIUDADANOS
           =============================================== -->

      <!--
        v-else-if significa:

        si la condición anterior NO se cumplió,
        comprobamos esta.

        Si currentPage === "ciudadanos",
        mostramos CitizensView.
      -->
      <CitizensView
        v-else-if="currentPage === 'ciudadanos'"
      />


    </main>

  </div>

</template>


<style scoped>

/* ======================================================
   ESTRUCTURA GENERAL
   ====================================================== */

/*
  Colocamos:

  Sidebar | Contenido

  uno al lado del otro.
*/
.app-layout {

  display: flex;

  min-height: 100vh;
}


/* ======================================================
   CONTENIDO PRINCIPAL
   ====================================================== */

/*
  Esta es toda la zona situada
  a la derecha del Sidebar.
*/
.main-content {

  /*
    Ocupa todo el espacio restante.
  */
  flex: 1;


  /*
    Espacio interior.
  */
  padding: 40px;


  /*
    Fondo gris muy claro.
  */
  background-color: #f6f8fc;


  /*
    Evita problemas con el cálculo
    del ancho al utilizar padding.
  */
  box-sizing: border-box;
}

</style>