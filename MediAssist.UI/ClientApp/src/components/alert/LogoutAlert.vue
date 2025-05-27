<template>
  <div v-if="isVisible" class="modal-overlay">
    <div class="confirm-popup">
      <div class="modal-header">
        <button class="close-button" @click="cancel">
          <span>&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <h2 class="modal-title">{{ title }}</h2>
        <p class="modal-message">{{ message }}</p>
        <div class="modal-buttons">
          <button @click="cancel" class="cancel-button">Cancel</button>
          <button @click="confirm" class="confirm-button">Logout</button>
       </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    isVisible: {
      type: Boolean,
      required: true
    },
    title: {
      type: String,
      default: 'Confirmation'
    },
    message: {
      type: String,
      default: 'Are you sure?'
    }
  },
  methods: {
    confirm() {
      this.$emit('confirm');
    },
    cancel() {
      this.$emit('cancel');
    }
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: flex-start;
  z-index: 1000;
}

.confirm-popup {
  display: flex;
  gap: 1px;
  width: 60%;
  height: auto;
  top: 175px;
  padding: 25px 40px 40px 40px;
  border-radius: 17px;
  background: #ffffff;
  flex-direction: column;
  position: absolute;
  max-width: 400px;
}

.close-button {
  width: 32px;
  height: 32px;
  font-size: 35px;
  cursor: pointer;
  color: #aaa;
  border: none;
  background: none;
  float: right;
}
.close-button:hover {
  color: black;
}
.modal-message {
  width: 100%;
  height: auto;
}
.modal-title {  
  width: 100%;
  height: auto;
  font-style: normal;
}
.modal-buttons {
  display: flex;
  gap:30px;
}

.confirm-button,
.cancel-button {
  cursor: pointer;
  font-size: 1rem;
  width: 45%;
  border-radius: 8px;
  border: 1px solid #D9D9D9;
  display: flex;
  height: 48px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex: 1 0 0;
}
.confirm-button {
  background-color: #0066cc;
  color: var(--Text-Text-white, var(--bg-white, #FFF));
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 24px */
  border-radius: 8px;
}

.cancel-button {
  background-color: #f5f5f5;
  color: #1C1C1C;
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 24px */
  border-radius: 8px;
  border: 1px solid #D9D9D9;
}

.confirm-button:hover {
  background-color: #005bb5;
}

.cancel-button:hover {
  background-color: #e0e0e0;
}
</style>