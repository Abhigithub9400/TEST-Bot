<template>
    <div v-if="isVisible" class="modal-overlay">
      <div class="modal">
        <div class="modal-header">
          <button class="close-button" @click="cancel">
            <span>&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <img class="bold-essentional-ui-check" alt="success icon" src="@/assets/setting_alert.png"/>
          <h2 class="modal-title">{{ title }}</h2>
          <p class="modal-message">
            Please complete your profile and settings before starting the session to ensure a smooth and personalized experience.
          </p>
          <div class="modal-buttons">
            <button @click="confirm" class="confirm-button">Update Settings</button>
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

.modal-body{
    border-radius: 17px 0px 0px 0px;
    text-align: center;
}

 .bold-essentional-ui-check{
    width: 74px;
    height: 74px;
 }

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
    padding: 0px 40px 40px 40px;
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
   left: 25px;
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

     .confirm-button {
      cursor: pointer;
      width: 100%;
      border: 1px solid #d9d9d9;
      display: flex;
      height: 48px;
      padding: 8px 20px;
      justify-content: center;
      align-items: center;
      gap: 8px;
      background-color: #06c;
      color: var(--Text-Text-white, var(--bg-white, #fff));
      font-size: 16px;
      font-style: normal;
      font-weight: 600;
      line-height: 150%;
      border-radius: 8px;
     }

 .confirm-button:hover {
   background-color: #005bb5;
 }

 @media (max-width: 575px) { 
  .modal{
    width: 60%;
    padding: 30px;
  }
 
}
 </style>