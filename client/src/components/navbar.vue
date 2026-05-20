<script setup>
import { totalItemsCount } from '../store/cart'
</script>

<template>
  <nav class="chrome-navbar">
    <div class="nav-container">
      
      <RouterLink to="/" class="brand-logo">
        M/G <span class="brand-accent">STUDIO</span>
      </RouterLink>

      <div class="nav-links">
        <RouterLink to="/" class="nav-item">Kolekcja</RouterLink>
        <RouterLink to="/admin" class="nav-item">Panel Admina</RouterLink>
      </div>

      <div class="nav-actions">
        <RouterLink to="/cart" class="cart-link">
          Koszyk
          <span v-if="totalItemsCount > 0" class="cart-badge">
            {{ totalItemsCount }}
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