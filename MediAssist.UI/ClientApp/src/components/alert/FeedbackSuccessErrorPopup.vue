<template>
    <div v-if="isVisible" class="modal-overlay">
      <div class="modal">
        <div class="modal-header">
          <button class="close-button" @click="cancel">
            <span>&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <div v-if="isError" class="error-icon">
         <img class="alert-image" src="@/assets/feedbackError.png" alt="Error Icon"/>
        </div>
        <div v-else class="success-icon">
          <img src="@/assets/feedbackSuccess.png" alt="Success Icon"/>
        </div>
          <h2 class="modal-title">{{ title }}</h2>
          <p class="modal-message">{{ message }}</p>
          <div class="modal-buttons" v-if="isError">
          <button @click="confirm" class="confirm-button">{{ tryAgain }}</button>
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
    tryAgain: {
      type: String,
      default: 'Retry'
    },
    cancelText: {
      type: String,
      default: 'Cancel'
    },
    title: {
      type: String,
      default: ''
    },
    message: {
      type: String,
      default: ''
    },
    isError: {
      type: Boolean,
      default: false
    
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
   
  display: flex;
  width: auto;
  height: auto;
  padding: 39px 40px 40px 40px;
  flex-direction: column;
  align-items: center;
  gap: 3px;
  flex-shrink: 0;
  border-radius: 17px;
  background: var(--bg-white, #FFF);
  margin-top: 150px;
  max-width: 350px;
}
  
 .close-button {
   width: 32px;
   height: 32px;
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
    width: auto;
    color: #1C1C1C;
    text-align: center;
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 150%; 

}

 .modal-title {
   color: #1C1C1C;
   font-size: 30px;
   font-style: normal;
   font-weight: 700;
   line-height: 90%;
   display:flex;
   justify-content: space-around;
 }
 .modal-buttons {
   display: flex;
   gap:30px;
 }
  
 .confirm-button {
  display: inline-flex;
  height: 50px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  background: var(--Primary-Colors, #0066D4);
  border: none;
  color: var(--Text-Text-white, var(--bg-white, #FFF));
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 24px */
  position: relative;
 }
 .modal-header{
   width: 100%;
 }
 .confirm-button:hover {
   background-color: #005bb5;
 }

 .success-icon {
  width: 76px;
  height: 76px;
 }

 .error-icon {
  width: 76px;
  height: 76px;
 }
 .alert-image{
  width: 100%;
    height: 100%;
 }
 .modal-body{
  display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    align-content: center;
    justify-content: center;
    align-items: center;
 }
 @media (max-width: 575px) { 
  .modal{
    width: 70%;
    padding: 25px;
    height: auto;
    }
 .modal-body{
   width: 100%;
 }
 .error-icon{
  width: 40px;
  height: 40px;
 }
 .modal-title{
  margin-top: 10px;
    font-size: 28px;
 }
 .modal-message{
  margin-top: 2px;
 }
}
 </style>