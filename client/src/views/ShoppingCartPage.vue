<script setup>
import { cartActions, cartState } from '@/store/cart';
import { computed } from 'vue'
import Navbar from '@/components/navbar.vue'
import CartItemCard from '@/components/CartItemCard.vue'

//lokalna referencja
const cartItems = computed(()=>cartState.items)

const increaseQuantity=(item)=>cartActions.increaseQuantity(item.id)
const decreaseQuantity = (item) => cartActions.decreaseQuantity(item.id)
const removeFromCart = (id) => cartActions.removeFromCart(id)

const proceedToCheckout = () => {
  alert(`Przejście do kasy.`)
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
          <CartItemCard 
            v-for="item in cartItems" 
            :key="item.id" 
            :item="item"
            :on-remove="removeFromCart"
            :on-increase-qty="increaseQuantity"
            :on-decrease-qty="decreaseQuantity"
          />
        </div>

       

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

<style scoped src="./ShoppingCartPage.css"></style>