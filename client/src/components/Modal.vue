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

<style scoped src="./Modal.css"></style>
