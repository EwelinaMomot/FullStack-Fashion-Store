<script setup>
import { totalItemsCount } from '../store/cart'
</script>

<template>
  <nav class="chrome-navbar">
    <div class="nav-container">
      
      <RouterLink to="/" class="brand-logo">
        E/M <span class="brand-accent">STUDIO</span>
      </RouterLink>

      <div class="nav-links">
        <RouterLink to="/" class="nav-item">Kolekcja</RouterLink>
        <RouterLink to="/admin" class="nav-item">Panel Admina</RouterLink>
      </div>

      <div class="nav-actions">
        <RouterLink to="/newuser" class="nav-item" style="padding-right: 20px;">Zaloguj się</RouterLink>
       <RouterLink to="/cart" class="cart-link">
  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
    <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"></path>
    <line x1="3" y1="6" x2="21" y2="6"></line>
    <path d="M16 10a4 4 0 0 1-8 0"></path>
  </svg>
  
  <span v-if="totalItemsCount > 0" class="cart-badge">
    ({{ totalItemsCount }})
  </span>
</RouterLink>
      </div>

    </div>
    <div class="navbar-bottom-edge"></div>
  </nav>
</template>

<style scoped src="./navbar.css"></style>