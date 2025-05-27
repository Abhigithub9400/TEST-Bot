import { ref, onMounted, onUnmounted, watch } from 'vue';
import { useRoute } from 'vue-router';

const isScrolled = ref(false);

export function useScrollHeader() {
  const route = useRoute();

  const handleScroll = () => {
    isScrolled.value = window.scrollY > 80;
    const header = document.querySelector('.main-header');
    const branding = document.querySelector('#branding');
    if (header) {
      if (isScrolled.value) {
        branding?.style.setProperty("color", '#163666');
        header.classList.add('scrolled');
      } else {
        branding?.style.setProperty("color", 'white');
        header.classList.remove('scrolled');
      }
    }
  };

  const enableScrollListener = () => {
    window.addEventListener('scroll', handleScroll);
    handleScroll();
  };

  const disableScrollListener = () => {
    window.removeEventListener('scroll', handleScroll);
  };

  onMounted(() => {
    enableScrollListener();
  });

  onUnmounted(() => {
    disableScrollListener();
  });

  watch(
    () => route.fullPath,
    (newPath, oldPath) => {
      if (newPath !== oldPath) {
        isScrolled.value = false;
        handleScroll();
      }
    }
  );

  return {
    isScrolled
  };
}
