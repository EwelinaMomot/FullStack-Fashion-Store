<script setup>
defineProps({
  item: {
    type: Object,
    required: true
  },
  onRemove: {
    type: Function,
    required: true
  },
  onIncreaseQty: {
    type: Function,
    required: true
  },
  onDecreaseQty: {
    type: Function,
    required: true
  }
})
</script>

<template>
  <article class="cart-item-card">
    <div class="item-media">
      <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.title" />
      <div v-else class="placeholder">Brak zdjęcia</div>
      <div class="silver-edge-vertical"></div>
    </div>

    <div class="item-details">
      <div class="item-header">
        <h4 class="item-title">{{ item.title }}</h4>
        <button @click="onRemove(item.id)" class="btn-remove" title="Usuń produkt">
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
        </button>
      </div>
      
      <p class="item-desc">{{ item.description }}</p>

      <div class="item-footer">
        <div class="quantity-selector">
          <button @click="onDecreaseQty(item)" class="qty-btn" :disabled="item.quantity <= 0">-</button>
          <span class="qty-value">{{ item.quantity || 1 }}</span>
          <button @click="onIncreaseQty(item)" class="qty-btn">+</button>
        </div>
      </div>
    </div>
  </article>
</template>

<style scoped src="./CartItemCard.css"></style>
