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
         <img src="" alt=""/>
        </div>
        <div v-else class="success-icon">
          <img src="@/assets/Success.png" alt="Sucess Icon"/>
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
  width: 496px;
  height: 261px;
  padding: 39px 40px 78px 40px;
  flex-direction: column;
  align-items: center;
  gap: 16px;
  flex-shrink: 0;
  border-radius: 17px;
  background: var(--bg-white, #FFF);
  margin-top: 150px;
}
  
 .close-button {
   width: 32px;
   height: 32px;
   padding: 8px;
   position: absolute;
   right: 24px;
   font-size: 35px;
   cursor: pointer;
   color: #aaa;
   border: none;
   background: none;
    left: 986px;
 }
 .close-button:hover {
   color: black;
 }
 .modal-message {
    width: 498px;
    color: #1C1C1C;
    text-align: center;
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 150%; 

}

 .modal-title {
   color: #1C1C1C;
   font-size: 34px;
   font-style: normal;
   font-weight: 700;
   line-height: 150%;
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
  position: absolute;
  right: 709px;
 }
 
  
 .confirm-button:hover {
   background-color: #005bb5;
 }

 .success-icon {
  width: 76px;
  height: 76px;
  margin-left: 196px;
 }

 .error-icon {
  width: 76px;
  height: 76px;
  margin-left: 208px;
 }
 
 </style>