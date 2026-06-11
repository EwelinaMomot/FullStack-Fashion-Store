<script setup>
import { ref, onMounted } from 'vue'
import Navbar from '@/components/navbar.vue'
import ProductAdminPanel from '@/components/ProductAdminPanel.vue'
import ProductInfoDetails from '@/components/ProductInfoDetails.vue'
import { productService } from '@/services/ProductService'
import { commentService } from '@/services/CommentService'
import { mapToCommentListDto, mapToNewCommentDto } from '@/mappers/CommentMapper'
import { jwtDecode } from 'jwt-decode'

const props = defineProps({
  productId: {
    type: String, 
    required: true
  }
})

const product = ref(null)
const isAdmin = ref(false)
const currentUserId = ref(null)
const newComment = ref('')
const comments = ref([])
const commentPage = ref(1)
const commentPageSize = 10
const commentTotalPages = ref(1)
const editingCommentId = ref(null)
const editedCommentText = ref('')

const getCurrentUserId = () => {
  const token = localStorage.getItem('token')
  if (!token) return null
  try {
    const decoded = jwtDecode(token)

    return Number( decoded.nameid) || null
  } catch (error) {
    return null
  }
}

const isOwnComment = (comment) => {
  return currentUserId.value != null && comment.creatorUserId === currentUserId.value
}

const fetchProductData = async (id) => {
  try {
    const response = await productService.getSingleProduct(id)
    product.value = response
  } catch(error) {
    alert("Nie udało się załadować produktu:", error)
  }
}

const fetchComments = async (page = 1) => {
  try {
    const response = await commentService.getComments(Number(props.productId), page, commentPageSize)
    comments.value = mapToCommentListDto(response.comments ?? response.data ?? [])
    commentPage.value = response.currentPage ?? page
    commentTotalPages.value = response.totalPages ?? 1
  } catch (error) {
    alert('Nie udało się pobrać komentarzy:', error)
  }
}

const submitComment = async () => {
  if (!newComment.value.trim()) {
    alert('Napisz komentarz przed wysłaniem.')
    return
  }

  if (!currentUserId.value) {
    alert('Musisz być zalogowany, aby dodać komentarz.')
    return
  }

  try {
    const payload = mapToNewCommentDto({
      productId: Number(props.productId),
      description: newComment.value.trim(),
      creatorUserId: currentUserId.value,
    })

    await commentService.addComment(payload)
    newComment.value = ''
    await fetchComments(1)
  } catch (error) {
    alert('Nie udało się dodać komentarza:', error)
  }
}

const startEditComment = (comment) => {
  editingCommentId.value = comment.id
  editedCommentText.value = comment.description
}

const cancelEditComment = () => {
  editingCommentId.value = null
  editedCommentText.value = ''
}

const saveEditedComment = async (commentId) => {
  if (!editedCommentText.value.trim()) {
    alert('Napisz treść komentarza.');
    return;
  }

  try {
    await commentService.editComment(commentId, { description: editedCommentText.value.trim() })
    editingCommentId.value = null
    editedCommentText.value = ''
    await fetchComments(commentPage.value)
  } catch (error) {
    alert('Nie udało się edytować komentarza:', error)
  }
}

const deleteComment = async (commentId) => {
  if (!confirm('Czy na pewno chcesz usunąć ten komentarz?')) {
    return
  }

  try {
    await commentService.deleteComment(commentId)
    await fetchComments(commentPage.value)
  } catch (error) {
    alert('Nie udało się usunąć komentarza:', error)
  }
}

const checkUserRole = () => {
  const token = localStorage.getItem('token')
  if (token) {
    try {
      const decodedPayload = jwtDecode(token)
      isAdmin.value = decodedPayload.role === 'Admin'
    } catch (error) {
      console.error("Nieprawidłowy token JWT", error)
    }
  }
}

const handleProductUpdated = async () => {
  await fetchProductData(props.productId)
}

onMounted(async () => {
  currentUserId.value = getCurrentUserId()

  try {
    await fetchProductData(props.productId)
    await fetchComments(1)
  } catch (e) {
    console.log('błąd podczas pobierania danych z serwera', e)
  }
  checkUserRole()
})
</script>

<template>
  <div class="chrome-page-wrapper">
    <Navbar />
    <div v-if="product" class="product-container">
      
      <div class="image-section">
        <div class="chrome-card">
          <div class="metallic-border-overlay"></div>
          <img 
            :src="product.imageUrl || 'https://via.placeholder.com/600x800'" 
            :alt="product.title" 
            class="product-image"
          />
        </div>
      </div>

      <div class="info-section">
        <ProductInfoDetails 
          :product="product" 
          :productId="props.productId" 
        />

        <ProductAdminPanel 
          v-if="isAdmin" 
          :productId="props.productId" 
          :product="product" 
          @product-updated="handleProductUpdated"
        />

        <section class="comments-section">
          <h3>Komentarze</h3>

          <div v-if="comments.length > 0" class="comments-list">
            <article v-for="comment in comments" :key="comment.id" class="comment-item">
              <div v-if="editingCommentId === comment.id">
                <textarea
                  v-model="editedCommentText"
                  class="chrome-input chrome-textarea"
                  rows="4"
                ></textarea>
                <div class="comment-actions">
                  <button type="button" class="btn-chrome-secondary" @click="cancelEditComment">Anuluj</button>
                  <button type="button" class="btn-chrome-action" @click="saveEditedComment(comment.id)">Zapisz</button>
                </div>
              </div>
              <div v-else>
                <p class="comment-text">{{ comment.description }}</p>
                <p class="comment-date">{{ comment.creationDate ? new Date(comment.creationDate).toLocaleString('pl-PL') : 'Brak daty' }}</p>
                <div v-if="isOwnComment(comment)" class="comment-actions">
                  <button type="button" class="btn-chrome-secondary" @click="startEditComment(comment)">Edytuj</button>
                  <button type="button" class="btn-chrome-delete" @click="deleteComment(comment.id)">Usuń</button>
                </div>
              </div>
            </article>
          </div>
          <p v-else class="no-comments">Brak komentarzy. Bądź pierwszy, który doda swoją opinię.</p>

          <div class="comment-form">
            <label for="new-comment" class="comment-label">Dodaj komentarz</label>
            <textarea
              id="new-comment"
              v-model="newComment"
              rows="4"
              placeholder="Napisz komentarz..."
              class="chrome-input chrome-textarea"
            ></textarea>
            <button type="button" class="btn-chrome-action" @click="submitComment">Dodaj komentarz</button>
          </div>
        </section>
      </div>
    </div>

    <div v-else class="loading-state">
      <div class="chrome-spinner"></div>
      <p>Pobieranie danych...</p>
    </div>
  </div>
</template>

<style scoped>
.chrome-page-wrapper {
  background: radial-gradient(circle at 50% 20%, var(--color-gradient-start) 0%, var(--color-gradient-mid) 40%, var(--color-gradient-end) 100%);
  min-height: 100vh;
  color: var(--color-text);
  padding: 0 1.5rem 4rem;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
}

.product-container {
  max-width: 1100px;
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
  margin-top: 2rem;
}

.chrome-card {
  background: #ffffff;
  border-radius: 32px;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(175, 205, 240, 0.35);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  background: #f4f8fd; 
  transition: transform 0.3s ease;
}

.product-image {
  width: 100%;
  height: auto;
  border-radius: 16px;
  object-fit: contain;
  max-height: 600px;
}

.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.comments-section {
  margin-top: 2rem;
  background: rgba(255, 255, 255, 0.96);
  border: 1px solid rgba(26, 115, 232, 0.15);
  border-radius: 24px;
  padding: 1.75rem;
}

.comments-section h3 {
  margin: 0 0 1rem 0;
  color: #1a73e8;
  font-size: 1.25rem;
}

.comments-list {
  display: grid;
  gap: 1rem;
}

.comment-item {
  background: #f6fbff;
  border: 1px solid rgba(26, 115, 232, 0.16);
  border-radius: 18px;
  padding: 1rem 1.25rem;
}

.comment-text {
  margin: 0 0 0.5rem 0;
  color: #202124;
}

.comment-date {
  margin: 0;
  color: #5f6368;
  font-size: 0.85rem;
}

.no-comments {
  margin: 0 0 1rem 0;
  color: #5f6368;
}

.comment-form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  margin-top: 1.25rem;
}

.comment-actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.75rem;
}

.btn-chrome-secondary {
  background: #ffffff;
  color: #1a73e8;
  border: 1px solid #1a73e8;
  padding: 0.7rem 1rem;
  border-radius: 9999px;
  cursor: pointer;
}

.btn-chrome-delete {
  background: #ffffff;
  color: #d93025;
  border: 1px solid #d93025;
  padding: 0.7rem 1rem;
  border-radius: 9999px;
  cursor: pointer;
}

.btn-chrome-delete:hover,
.btn-chrome-secondary:hover {
  background: rgba(26, 115, 232, 0.08);
}

.comment-label {
  font-weight: 500;
  color: #1a73e8;
}

.comment-form .chrome-textarea {
  min-height: 120px;
}

.comment-form .btn-chrome-action {
  width: fit-content;
  align-self: flex-start;
}

.loading-state {
  text-align: center;
  color: #5f6368;
  font-weight: 300;
  margin-top: 4rem;
  font-size: 1.1rem;
}

@media (max-width: 900px) {
  .product-container {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
}
</style>