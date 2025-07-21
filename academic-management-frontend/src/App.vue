<script setup></script>
<template>
  <v-app>
    <v-navigation-drawer :width="258" app permanent>
      <v-list dense>
        <v-list-item title="Módulo Acadêmico" subtitle=""></v-list-item>
        <v-divider></v-divider>
        <v-list-item link title="Alunos" to="/students"> </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-main>
      <v-container>
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>
<style scoped></style>
