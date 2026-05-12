const services = [
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
     
      head: "مؤقت"
    },
    {
      url: "./images/dylan-mcleod-Q81AduLKMMc-unsplash.jpg",
   
      head: "مؤقت"
    }
  ];

  const servParent = document.getElementById("services-slider-wrapper");

  // Create and append service cards
  services.forEach(service => {
    const card = document.createElement("div");
    card.className = "service-card";
    card.style.backgroundImage = `url(${service.url})`;

    const textWrapper = document.createElement("div");

    const moto = document.createElement("p");
    moto.textContent = service.moto;

    const head = document.createElement("h2");
    head.textContent = service.head;

    card.appendChild(moto);
    card.appendChild(head);
    servParent.appendChild(card);
  });

  // Add scroll functionality to arrows
  const servLeft = document.getElementById("slider-left-arrow");
  const servRight = document.getElementById("slider-right-arrow");

  servRight.addEventListener("click", () => {
    servParent.scrollLeft += servParent.offsetWidth;
  });

  servLeft.addEventListener("click", () => {
    servParent.scrollLeft -= servParent.offsetWidth;
  });

  const sliderWrapper = document.querySelector('.modern-slider-wrapper');
  const sliderItems = document.querySelectorAll('.modern-slider-item');
  const nextSlideButton = document.querySelector('.modern-slider-nav.next');
  const prevSlideButton = document.querySelector('.modern-slider-nav.prev');
  const sliderDots = document.querySelectorAll('.modern-slider-dots span');

  let activeIndex = 0;
  const totalItems = sliderItems.length;

  // Update Slider
  function updateSliderPosition() {
      sliderWrapper.style.transform = `translateX(-${activeIndex * 320}px)`;
      updateDots();
  }

  document.querySelectorAll('.slider-container').forEach(container => {
  const slider = container.querySelector('.slider');
  const cards = slider.children;
  const nextButton = container.querySelector('.slider-nav.next');
  const prevButton = container.querySelector('.slider-nav.prev');
  
  const cardWidth = cards[0].offsetWidth + 20; // Card width + gap
  let currentIndex = 1;

  // Clone first and last slides
  const firstClone = cards[0].cloneNode(true);
  const lastClone = cards[cards.length - 1].cloneNode(true);
  slider.appendChild(firstClone);
  slider.insertBefore(lastClone, cards[0]);

  // Set initial position
  slider.style.transform = `translateX(-${cardWidth}px)`;

  // Update position function
  function updateSliderPosition() {
      slider.style.transition = 'transform 0.5s ease-in-out';
      slider.style.transform = `translateX(-${currentIndex * cardWidth}px)`;
  }

  // Handle infinite loop transitions
  slider.addEventListener('transitionend', () => {
      const slides = slider.children;
      if (slides[currentIndex].classList.contains('clone-first')) {
          slider.style.transition = 'none';
          currentIndex = 1;
          slider.style.transform = `translateX(-${cardWidth}px)`;
      } else if (slides[currentIndex].classList.contains('clone-last')) {
          slider.style.transition = 'none';
          currentIndex = slides.length - 2;
          slider.style.transform = `translateX(-${currentIndex * cardWidth}px)`;
      }
  });

  // Next button logic
  nextButton.addEventListener('click', () => {
      const totalSlides = slider.children.length - 2; // Exclude clones
      if (currentIndex < totalSlides + 1) {
          currentIndex++;
          updateSliderPosition();
      }
  });

  // Previous button logic
  prevButton.addEventListener('click', () => {
      if (currentIndex > 0) {
          currentIndex--;
          updateSliderPosition();
      }
  });
});


  // Update Active Dots
  function updateDots() {
      sliderDots.forEach((dot, index) => {
          dot.classList.toggle('active', index === activeIndex);
      });
  }

  // Next Slide
  nextSlideButton.addEventListener('click', () => {
      activeIndex = (activeIndex + 1) % totalItems;
      updateSliderPosition();
  });

  // Previous Slide
  prevSlideButton.addEventListener('click', () => {
      activeIndex = (activeIndex - 1 + totalItems) % totalItems;
      updateSliderPosition();
  });

  // Dot Navigation
  sliderDots.forEach((dot, index) => {
      dot.addEventListener('click', () => {
          activeIndex = index;
          updateSliderPosition();
      });
  });

document.querySelectorAll('.slider-container').forEach(container => {
  const slider = container.querySelector('.slider');
  const cards = Array.from(slider.children);
  const nextButton = container.querySelector('.slider-nav.next');
  const prevButton = container.querySelector('.slider-nav.prev');

  const cardWidth = cards[0].offsetWidth + 20; // Width of a card plus gap
  let currentIndex = 0;

  // Clone first and last few cards for seamless infinite effect
  const totalCards = cards.length;
  const clonesBefore = [];
  const clonesAfter = [];

  for (let i = 0; i < 3; i++) { // Clone 3 cards at the start and end (adjust as needed)
      clonesBefore.push(cards[totalCards - 1 - i].cloneNode(true));
      clonesAfter.push(cards[i].cloneNode(true));
  }

  clonesBefore.reverse().forEach(clone => slider.insertBefore(clone, slider.firstChild));
  clonesAfter.forEach(clone => slider.appendChild(clone));

  // Set initial slider position
  const offset = clonesBefore.length * cardWidth;
  slider.style.transform = `translateX(-${offset}px)`;

  function updateSliderPosition() {
      slider.style.transition = 'transform 0.5s ease-in-out';
      slider.style.transform = `translateX(-${(currentIndex + clonesBefore.length) * cardWidth}px)`;
  }

  // Handle transition end for infinite effect
  slider.addEventListener('transitionend', () => {
      if (currentIndex < 0) {
          currentIndex = totalCards - 1;
          slider.style.transition = 'none';
          slider.style.transform = `translateX(-${(currentIndex + clonesBefore.length) * cardWidth}px)`;
      } else if (currentIndex >= totalCards) {
          currentIndex = 0;
          slider.style.transition = 'none';
          slider.style.transform = `translateX(-${(currentIndex + clonesBefore.length) * cardWidth}px)`;
      }
  });

  // Next button logic
  nextButton.addEventListener('click', () => {
      currentIndex++;
      updateSliderPosition();
  });

  // Previous button logic
  prevButton.addEventListener('click', () => {
      currentIndex--;
      updateSliderPosition();
  });
});