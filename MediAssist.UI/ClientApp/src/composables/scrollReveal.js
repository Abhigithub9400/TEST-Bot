import ScrollReveal from 'scrollreveal'

export default {
  mounted(el, binding) {
    ScrollReveal().reveal(el, binding.value || {
      origin: 'bottom',
      distance: '50px',
      duration: 800,
      delay: 200,
      reset: false,
    })
  }
}
