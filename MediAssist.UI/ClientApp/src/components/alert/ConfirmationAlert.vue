<template>
    <div v-if="isVisible" class="modal-overlay">
      <div class="modal">
        <div class="modal-header">
          <button class="close-button" @click="cancel">
            <span>&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <h2 class="modal-title">{{ title }}</h2>
          <p class="modal-message">{{ message }}</p>
          <div class="modal-buttons">
            <button @click="cancel" class="cancel-button">{{ cancelText }}</button>
            <button @click="confirm" class="confirm-button">{{ confirmText }}</button>
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
    },
    confirmText: {
      type: String,
      default: 'Confirm Action'
    },
    cancelText: {
      type: String,
      default: 'Cancel'
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
  
 .modal {
    max-width: 400px;
    padding: 25px 40px 40px 40px;
    gap: 3px;
    border-radius: 17px;
    background: #ffffff;
    flex-direction: column;
    width: auto;
    height: auto;
    display: flex;
    margin-top: 150px;
}
 
  
 .close-button {
   width: 32px;
   height: 32px;
   padding: 8px;
   position: relative;
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
    height: 100%;
    width: 100%;
}

 .modal-title {
    width: 100%;
    height: 100%;
   font-style: normal;
   margin-top: 3px;
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

 @media (max-width: 575px) { 
  .modal{
    width: 60%;
    padding: 30px;
  }
 
}
 </style>