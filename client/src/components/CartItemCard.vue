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

<style scoped>
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
  position: relative;
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

.silver-edge-vertical {
  height: 100%;
  width: 2px;
  background: linear-gradient(180deg, transparent, rgba(160, 195, 235, 0.3), transparent);
  position: absolute;
  right: 0;
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
  background: none;
  border: none;
  color: #5f6368;
  cursor: pointer;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s ease;
  flex-shrink: 0;
}

.btn-remove:hover {
  color: #d93025;
}

.item-desc {
  font-size: 0.9rem;
  font-weight: 300;
  color: #5f6368;
  margin: 0.5rem 0 1rem 0;
  line-height: 1.5;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.quantity-selector {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: #f1f3f4;
  border-radius: 24px;
  padding: 0.4rem 0.8rem;
}

.qty-btn {
  background: none;
  border: none;
  color: #1a73e8;
  cursor: pointer;
  font-weight: 600;
  font-size: 1.1rem;
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: all 0.2s ease;
}

.qty-btn:hover:not(:disabled) {
  background: #e8eaed;
}

.qty-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.qty-value {
  min-width: 30px;
  text-align: center;
  font-weight: 500;
  color: #202124;
}
</style>
