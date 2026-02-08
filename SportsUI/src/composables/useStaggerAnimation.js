export function useStaggerAnimation() {
  const onBeforeEnter = (el) => {
    el.style.opacity = 0
    el.style.transform = 'translateY(12px)'
  }

  const onEnter = (el, done) => {
    const delay = el.dataset.index * 40
    setTimeout(() => {
      el.style.transition = 'opacity 0.4s ease, transform 0.4s ease'
      el.style.opacity = 1
      el.style.transform = 'translateY(0)'
      setTimeout(done, 400)
    }, delay)
  }

  return { onBeforeEnter, onEnter }
}
