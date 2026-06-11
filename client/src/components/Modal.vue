<script setup>
defineProps({
  isOpen: {
    type: Boolean,
    required: true
  },
  title: {
    type: String,
    required: true
  },
  onClose: {
    type: Function,
    required: true
  },
  submitButtonText: {
    type: String,
    default: 'Zapisz'
  },
  cancelButtonText: {
    type: String,
    default: 'Anuluj'
  }
})

defineEmits(['submit', 'close'])
</script>

<template>
  <div v-if="isOpen" class="modal-overlay" @click.self="onClose">
    <div class="modal-content">
      <div class="modal-header">
        <h3 class="modal-title">{{ title }}</h3>
        <button type="button" class="btn-close" @click="onClose">&times;</button>
      </div>
      
      <slot name="body"></slot>

      <div class="modal-actions">
        <button type="button" class="btn-modal-cancel" @click="onClose">{{ cancelButtonText }}</button>
        <button type="submit" class="btn-modal-save" @click="$emit('submit')">{{ submitButtonText }}</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: #ffffff;
  border-radius: 32px;
  padding: 2rem;
  max-width: 600px;
  width: 100%;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid rgba(160, 195, 235, 0.3);
}

.modal-title {
  font-size: 1.5rem;
  font-weight: 500;
  color: #202124;
  margin: 0;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  color: #5f6368;
  cursor: pointer;
  padding: 0;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s ease;
}

.btn-close:hover {
  color: #202124;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid rgba(160, 195, 235, 0.3);
}

.btn-modal-cancel, .btn-modal-save {
  padding: 0.8rem 1.5rem;
  border-radius: 24px;
  font-family: inherit;
  font-size: 0.95rem;
  font-weight: 500;
  cursor: pointer;
  border: none;
  transition: all 0.2s ease;
}

.btn-modal-cancel {
  background: #e8eaed;
  color: #3c4043;
}

.btn-modal-cancel:hover {
  background: #dadce0;
}

.btn-modal-save {
  background: #1a73e8;
  color: #ffffff;
  box-shadow: 0 4px 15px rgba(26, 115, 232, 0.3);
}

.btn-modal-save:hover {
  background: #1557b0;
  box-shadow: 0 6px 20px rgba(26, 115, 232, 0.4);
  transform: translateY(-2px);
}
</style>
