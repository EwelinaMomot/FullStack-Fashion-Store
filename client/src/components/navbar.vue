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

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600&display=swap');

.chrome-navbar {
  font-family: 'Outfit', sans-serif;
  background: transparent;
  padding: 2rem 0;
  width: 100%;
  
}

.nav-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
  display: flex;
  justify-content: space-between; 
  align-items: center;
}


.brand-logo {
  text-decoration: none;
  color: #000000; 
  font-size: 1.25rem;
  font-weight: 500;
  letter-spacing: 0.5px;
}

.brand-accent {
  font-weight: 300;
}


.right-menu {
  display: flex;
  align-items: center;
  gap: 2.5rem; }

.nav-links {
  display: flex;
  gap: 2.5rem;}


.nav-item, .cart-link {
  text-decoration: none;
  color: #000000; 
  font-size: 1rem;
  font-weight: 400;
  transition: opacity 0.2s ease;
}

.nav-item:hover, .cart-link:hover {
  opacity: 0.5; 
}


.cart-badge {
  font-weight: 500;
  margin-left: 0.15rem;
}
</style>