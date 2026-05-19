<script setup>
import { cartActions, cartState, totalPrice } from '@/store/cart';
import { ref,onMounted,computed } from 'vue'
import {cartActions,cartState,totalPrice} from '../store/cart'
import { it } from 'vue-router/dist/index-BzEKChPW.js';
//lokalna referencja
const cartItems = computed(()=>cartState.items)
const cartTotal = computed(()=>totalPrice.value)

const increaseQuantity=(item)=>cartActions.increaseQuantity(item.id)
const decreaseQuantity = (item) => cartActions.decreaseQuantity(item.id)
const removeFromCart = (id) => cartActions.removeFromCart(id)

const proceedToCheckout = () => {
  alert(`Proceeding to checkout. Total amount to pay: ${cartTotal.value} zł`)
  // TODO
}

</script>

<template>
  <div class="cart-page-wrapper">
    <div class="cart-container">
      <header class="cart-header">
        <h1 class="page-title">Shopping Bag <span class="item-count">({{ cartItems.length }})</span></h1>
        <RouterLink to="/" class="back-shopping">← Continue Shopping</RouterLink>
      </header>

      <div v-if="cartItems.length > 0" class="cart-content-layout">
        
        <div class="cart-items-list">
          <article v-for="item in cartItems" :key="item.id" class="cart-item-card">
            
            <div class="item-media">
              <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.title" />
              <div v-else class="placeholder">No Image</div>
              <div class="silver-edge-vertical"></div>
            </div>

            <div class="item-details">
              <div class="item-header">
                <h4 class="item-title">{{ item.title }}</h4>
                <button @click="removeFromCart(item.id)" class="btn-remove" title="Remove item">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
                </button>
              </div>
              
              <p class="item-desc">{{ item.description }}</p>

              <div class="item-footer">
                <div class="quantity-selector">
                  <button @click="decreaseQuantity(item)" class="qty-btn" :disabled="item.quantity <= 1">-</button>
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
            <h3 class="summary-title">Summary</h3>
            
            <div class="summary-row">
              <span>Subtotal</span>
              <span>{{ cartTotal }} zł</span>
            </div>
            <div class="summary-row">
              <span>Shipping</span>
              <span class="shipping-free">FREE</span>
            </div>

            <div class="summary-divider"></div>

            <div class="summary-row total-row">
              <span>Total (incl. VAT)</span>
              <span class="final-price">{{ cartTotal }} zł</span>
            </div>

            <button @click="proceedToCheckout" class="btn-checkout">
              <span>Proceed to Checkout</span>
              <div class="btn-glow"></div>
            </button>
            
            <div class="secure-badge">
              <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect><path d="M7 11V7a5 5 0 0 1 10 0v4"></path></svg>
              Secure SSL Encrypted Checkout
            </div>
          </div>
        </aside>

      </div>

      <div v-else class="empty-cart-state">
        <div class="empty-icon-box">
          <div class="chrome-ring"></div>
          <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" class="bag-icon"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"></path><line x1="3" y1="6" x2="21" y2="6"></line><path d="M16 10a4 4 0 0 1-8 0"></path></svg>
        </div>
        <h2>Your bag is empty</h2>
        <p>Items you add to your shopping bag will appear here.</p>
        <RouterLink to="/" class="btn-back-home">Explore Collection</RouterLink>
      </div>

    </div>
  </div>
</template>

<style scoped>

.cart-page-wrapper {
  background-color: #0d0f12; 
  color: #ffffff;
  min-height: 100vh;
  padding: 4rem 2rem;
  font-family: 'Inter', sans-serif;
}

.cart-container {
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
}


.cart-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  padding-bottom: 1.5rem;
  margin-bottom: 3rem;
}

.page-title {
  font-size: 2rem;
  font-weight: 700;
  letter-spacing: -0.02em;
  text-transform: uppercase;
  margin: 0;
}

.item-count {
  color: #7a7e85;
  font-weight: 400;
}

.back-shopping {
  color: #a0aec0;
  text-decoration: none;
  font-size: 0.9rem;
  transition: color 0.2s ease;
  letter-spacing: 0.05em;
}

.back-shopping:hover {
  color: #ffffff;
}


.cart-content-layout {
  display: grid;
  grid-template-columns: 1fr 380px;
  gap: 3rem;
  align-items: flex-start;
}

@media (max-width: 968px) {
  .cart-content-layout {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
}


.cart-items-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.cart-item-card {
  position: relative;
  background: linear-gradient(180deg, #181b1f 0%, #121418 100%);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 16px 4px 16px 4px; 
  padding: 1.5rem;
  display: flex;
  gap: 1.5rem;
  overflow: hidden;
}


.item-media {
  position: relative;
  width: 120px;
  height: 140px;
  background: #1b1e22;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  border-radius: 8px;
  overflow: hidden;
}

.item-media img {
  max-width: 100%;
  max-height: 100%;
  object-fit: cover;
}

.silver-edge-vertical {
  position: absolute;
  top: 0; right: 0; bottom: 0;
  width: 1px;
  background: linear-gradient(180deg, transparent, rgba(255,255,255,0.3), transparent);
}


.item-details {
  display: flex;
  flex-direction: column;
  flex-grow: 1;
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 0.5rem;
}

.item-title {
  font-size: 1.15rem;
  font-weight: 600;
  margin: 0;
  text-transform: uppercase;
  letter-spacing: 0.02em;
}

.btn-remove {
  background: transparent;
  border: none;
  color: #5a5e65;
  cursor: pointer;
  transition: color 0.2s ease;
  padding: 0;
}

.btn-remove:hover {
  color: #e53e3e;
}

.item-desc {
  color: #7a7e85;
  font-size: 0.88rem;
  line-height: 1.4;
  margin: 0 0 auto 0;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 1rem;
}


.quantity-selector {
  display: flex;
  align-items: center;
  background: #0d0f12;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 4px;
}

.qty-btn {
  background: transparent;
  border: none;
  color: #ffffff;
  width: 32px;
  height: 32px;
  cursor: pointer;
  font-size: 1rem;
  transition: background 0.2s;
}

.qty-btn:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.05);
}

.qty-btn:disabled {
  color: #3a3e45;
  cursor: not-allowed;
}

.qty-value {
  padding: 0 10px;
  font-size: 0.9rem;
  min-width: 20px;
  text-align: center;
}

.price-total {
  font-size: 1.2rem;
  font-weight: 700;
  letter-spacing: -0.01em;
}


.cart-summary-sidebar {
  position: sticky;
  top: 2rem;
}

.summary-card {
  background: linear-gradient(180deg, #181b1f 0%, #121418 100%);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 16px 4px 16px 4px;
  padding: 2rem;
}

.summary-title {
  font-size: 1.3rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin-top: 0;
  margin-bottom: 1.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  padding-bottom: 1rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.95rem;
  color: #a0aec0;
  margin-bottom: 1rem;
}

.shipping-free {
  color: #48bb78;
  font-weight: 700;
  letter-spacing: 0.05em;
}

.summary-divider {
  height: 1px;
  background: rgba(255, 255, 255, 0.08);
  margin: 1.5rem 0;
}

.total-row {
  color: #ffffff;
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 2rem;
}

.final-price {
  font-size: 1.4rem;
  font-weight: 800;
}


.btn-checkout {
  position: relative;
  width: 100%;
  background: #ffffff;
  color: #000000;
  border: none;
  padding: 1.1rem;
  font-size: 0.9rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.15em;
  cursor: pointer;
  border-radius: 4px;
  transition: transform 0.2s, box-shadow 0.2s;
}

.btn-checkout:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(255, 255, 255, 0.1);
}

.secure-badge {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-size: 0.75rem;
  color: #5a5e65;
  margin-top: 1.2rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
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
  position: relative;
  margin-bottom: 2rem;
}

.chrome-ring {
  position: absolute;
  top: -10px; left: -10px; right: -10px; bottom: -10px;
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 50%;
  background: radial-gradient(circle, rgba(255,255,255,0.02) 0%, transparent 70%);
}

.bag-icon {
  color: #3a3e45;
}

.empty-cart-state h2 {
  font-size: 1.75rem;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 0.02em;
}

.empty-cart-state p {
  color: #7a7e85;
  margin-bottom: 2.5rem;
}

.btn-back-home {
  display: inline-block;
  border: 1px solid rgba(255,255,255,0.2);
  color: #ffffff;
  text-decoration: none;
  padding: 1rem 2.5rem;
  font-size: 0.85rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.15em;
  transition: background 0.2s, border 0.2s;
}

.btn-back-home:hover {
  background: #ffffff;
  color: #000000;
  border-color: #ffffff;
}
</style>