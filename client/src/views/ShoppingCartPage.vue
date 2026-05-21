<script setup>
import { cartActions, cartState, totalPrice } from '@/store/cart';
import { computed } from 'vue'
import Navbar from '@/components/navbar.vue'

//lokalna referencja
const cartItems = computed(()=>cartState.items)
const cartTotal = computed(()=>totalPrice.value)

const increaseQuantity=(item)=>cartActions.increaseQuantity(item.id)
const decreaseQuantity = (item) => cartActions.decreaseQuantity(item.id)
const removeFromCart = (id) => cartActions.removeFromCart(id)

const proceedToCheckout = () => {
  alert(`Przejście do kasy. Do zapłaty: ${cartTotal.value} zł`)
  // TODO
}

</script>

<template>
  <div class="cart-page-wrapper">
    <Navbar />
    <div class="cart-container">
      <header class="cart-header">
        <h1 class="page-title">Koszyk <span class="item-count">({{ cartItems.length }})</span></h1>
        
      </header>

      <div v-if="cartItems.length > 0" class="cart-content-layout">
        
        <div class="cart-items-list">
          <article v-for="item in cartItems" :key="item.id" class="cart-item-card">
            
            <div class="item-media">
              <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.title" />
              <div v-else class="placeholder">Brak zdjęcia</div>
              <div class="silver-edge-vertical"></div>
            </div>

            <div class="item-details">
              <div class="item-header">
                <h4 class="item-title">{{ item.title }}</h4>
                <button @click="removeFromCart(item.id)" class="btn-remove" title="Usuń produkt">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
                </button>
              </div>
              
              <p class="item-desc">{{ item.description }}</p>

              <div class="item-footer">
                <div class="quantity-selector">
                  <button @click="decreaseQuantity(item)" class="qty-btn" :disabled="item.quantity <= 0">-</button>
                  <span class="qty-value">{{ item.quantity || 1 }}</span>
                  <button @click="increaseQuantity(item)" class="qty-btn">+</button>
                </div>
                <div class="price-box">
                  <span class="price-total">{{ (item.price * (item.quantity || 1)) }} zł</span>
                </div>
              </div>
            </div>

          </article>
        </div>

        <aside class="cart-summary-sidebar">
          <div class="summary-card">
            <h3 class="summary-title">Podsumowanie</h3>
            
            <div class="summary-row">
              <span>Suma częściowa</span>
              <span>{{ cartTotal }} zł</span>
            </div>
            <div class="summary-row">
              <span>Dostawa</span>
              <span class="shipping-free">DARMOWA</span>
            </div>

            <div class="summary-divider"></div>

            <div class="summary-row total-row">
              <span>Razem </span>
              <span class="final-price">{{ cartTotal }} zł</span>
            </div>

            <button @click="proceedToCheckout" class="btn-checkout">
              <span>Przejdź do kasy</span>
              <div class="btn-glow"></div>
            </button>
            
          
          </div>
        </aside>

      </div>

      <div v-else class="empty-cart-state">
        <div class="empty-icon-box">
          <div class="chrome-ring"></div>
          <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" class="bag-icon"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"></path><line x1="3" y1="6" x2="21" y2="6"></line><path d="M16 10a4 4 0 0 1-8 0"></path></svg>
        </div>
        <h2>Twój koszyk jest pusty</h2>
        <p>Produkty dodane do koszyka pojawią się tutaj.</p>
        <RouterLink to="/" class="btn-back-home">Przeglądaj kolekcję</RouterLink>
      </div>

    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600&display=swap');

.cart-page-wrapper {
  font-family: 'Outfit', sans-serif;
  background: radial-gradient(circle at 50% 20%, #dcecfa 0%, #edf4fc 40%, #ffffff 100%);
  min-height: 100vh;
  color: #3c4043;
  padding: 0rem 1.5rem 4rem;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
}

.cart-container {
  max-width: 1200px;
  width: 100%;
}

.cart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 3rem;
}

.page-title {
  font-size: 2.25rem;
  font-weight: 500;
  color: #202124;
  margin: 0;
}

.item-count {
  font-weight: 300;
  color: #5f6368;
  font-size: 1.5rem;
}

.back-shopping {
  text-decoration: none;
  color: #1a73e8;
  font-weight: 500;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  border-radius: 9999px;
  transition: background-color 0.2s ease;
}

.back-shopping:hover {
  background-color: #e8f0fe;
}

.cart-content-layout {
  display: grid;
  grid-template-columns: 1fr 380px;
  gap: 3rem;
  align-items: start;
}


.cart-items-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.cart-item-card {
  display: flex;
  background: #ffffff;
  border-radius: 32px;
  padding: 1.5rem;
  box-shadow: 0 4px 20px rgba(201, 222, 250, 0.5);
  transition: box-shadow 0.3s ease;
}

.cart-item-card:hover {
  box-shadow: 0 6px 30px rgba(201, 222, 250, 0.7);
}

.item-media {
  flex: 0 0 120px;
  height: 120px;
  background: #f4f8fd;
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  margin-right: 1.5rem;
}

.item-media img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.placeholder {
  color: #a1c2fa;
  font-size: 0.85rem;
}

.item-details {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.item-title {
  font-size: 1.2rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 0.25rem 0;
}

.btn-remove {
  background: #fce8e6;
  color: #d93025;
  border: none;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.btn-remove:hover {
  background: #fad2cf;
}

.item-desc {
  font-size: 0.9rem;
  font-weight: 300;
  color: #5f6368;
  margin: 0 0 1rem 0;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.quantity-selector {
  display: flex;
  align-items: center;
  background: #f1f3f4;
  border-radius: 9999px;
  padding: 0.25rem;
}

.qty-btn {
  background: transparent;
  border: none;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  font-size: 1.2rem;
  color: #3c4043;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.2s;
}

.qty-btn:hover:not(:disabled) {
  background: #e8eaed;
}

.qty-btn:disabled {
  color: #bdc1c6;
  cursor: not-allowed;
}

.qty-value {
  padding: 0 0.75rem;
  font-weight: 500;
  font-size: 0.95rem;
  min-width: 20px;
  text-align: center;
}

.price-total {
  font-size: 1.15rem;
  font-weight: 600;
  color: #202124;
}


.cart-summary-sidebar {
  position: sticky;
  top: 2rem;
}

.summary-card {
  background: #ffffff;
  border-radius: 32px;
  padding: 2rem;
  box-shadow: 0 4px 20px rgba(201, 222, 250, 0.5);
}

.summary-title {
  font-size: 1.25rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 1.5rem 0;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  color: #5f6368;
  margin-bottom: 1rem;
  font-size: 1rem;
}

.shipping-free {
  color: #1e8e3e; 
  font-weight: 500;
}

.summary-divider {
  height: 1px;
  background: rgba(160, 195, 235, 0.3);
  margin: 1.5rem 0;
}

.total-row {
  font-size: 1.25rem;
  color: #202124;
  font-weight: 500;
  margin-bottom: 2rem;
  align-items: center;
}

.final-price {
  font-size: 1.5rem;
  font-weight: 600;
}

.btn-checkout {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #ffffff;
  color: #3c4043;
  border: 1px solid #c9defa;
  padding: 1rem;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 500;
  font-family: inherit;
  cursor: pointer;
  box-shadow: 0 4px 15px rgba(161, 194, 250, 0.4);
  transition: all 0.3s ease;
  margin-bottom: 1.5rem;
}

.btn-checkout:hover {
  box-shadow: 0 8px 25px rgba(161, 194, 250, 0.6);
  color: #1a73e8;
  transform: translateY(-2px);
}

.secure-badge {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  color: #5f6368;
  font-size: 0.85rem;
}


.empty-cart-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 6rem 2rem;
  text-align: center;
}

.empty-icon-box {
  background: #f4f8fd;
  color: #a1c2fa;
  width: 96px;
  height: 96px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1.5rem;
}

.empty-title {
  font-size: 2rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 0.5rem 0;
}

.empty-desc {
  color: #5f6368;
  font-weight: 300;
  font-size: 1.1rem;
  margin-bottom: 2rem;
}

.btn-back-home {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: #ffffff;
  color: #3c4043;
  border: none;
  padding: 1rem 2.5rem;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 500;
  text-decoration: none;
  box-shadow: 0 8px 24px rgba(160, 195, 235, 0.4);
  transition: all 0.3s ease;
}

.btn-back-home:hover {
  box-shadow: 0 12px 32px rgba(160, 195, 235, 0.6);
  color: #1a73e8;
  transform: translateY(-2px);
}


</style>