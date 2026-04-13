        // Setup Intersection Observer for reveal animations
        const observerOptions = {
            threshold: 0.1,
            rootMargin: "0px 0px -50px 0px"
        };

        const observer = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('active');
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        document.querySelectorAll('.reveal, .reveal-left, .reveal-right, .reveal-scale').forEach(el => observer.observe(el));

        // Mobile Menu Toggle Logic
        const mobileMenuBtn = document.getElementById('mobile-menu-btn');
        const mobileMenuIcon = document.getElementById('mobile-menu-icon');
        const mobileMenu = document.getElementById('mobile-menu');
        const navbarLayoutBase = document.querySelector('#navbar > div');
        let isMobileMenuOpen = false;

        function toggleMobileMenu() {
            isMobileMenuOpen = !isMobileMenuOpen;
            if (isMobileMenuOpen) {
                mobileMenu.classList.remove('scale-y-0', 'opacity-0', 'pointer-events-none');
                mobileMenu.classList.add('scale-y-100', 'opacity-100', 'pointer-events-auto');
                mobileMenuIcon.textContent = 'close';
                mobileMenuIcon.style.transform = 'rotate(90deg)';
                if (window.scrollY < 50) {
                    navbarLayoutBase.classList.remove('mt-4', 'rounded-full', 'glass');
                    navbarLayoutBase.classList.add('mt-0', 'rounded-b-2xl', 'bg-white/95', 'backdrop-blur-md', 'shadow-md');
                }
            } else {
                mobileMenu.classList.remove('scale-y-100', 'opacity-100', 'pointer-events-auto');
                mobileMenu.classList.add('scale-y-0', 'opacity-0', 'pointer-events-none');
                mobileMenuIcon.textContent = 'menu';
                mobileMenuIcon.style.transform = 'rotate(0deg)';
                if (window.scrollY < 50) {
                    navbarLayoutBase.classList.add('mt-4', 'rounded-full', 'glass');
                    navbarLayoutBase.classList.remove('mt-0', 'rounded-b-2xl', 'bg-white/95', 'backdrop-blur-md', 'shadow-md');
                }
            }
        }

        if (mobileMenuBtn) {
            mobileMenuBtn.addEventListener('click', toggleMobileMenu);
        }

        document.querySelectorAll('.mobile-menu-link').forEach(link => {
            link.addEventListener('click', () => {
                if (isMobileMenuOpen) toggleMobileMenu();
            });
        });

        // Parallax, Navbar, and Flow scroll logic
        const navbarLayout = document.querySelector('#navbar > div');
        const heroImg = document.querySelector('.parallax-hero');
        const journeyFlowLine = document.getElementById('journey-flow-line');
        const journeySection = document.getElementById('journey');
        const scrollToTopBtn = document.getElementById('scrollToTopBtn');

        let ticking = false;
        let lastScrollY = window.scrollY;
        let cachedWindowHeight = typeof window !== 'undefined' ? window.innerHeight : 0;
        let journeyTop = 0;
        let journeyHeight = 0;

        function cacheDimensions() {
            cachedWindowHeight = window.innerHeight;
            if (journeySection) {
                const rect = journeySection.getBoundingClientRect();
                journeyTop = rect.top + window.scrollY;
                journeyHeight = rect.height;
            }
        }

        window.addEventListener('resize', cacheDimensions);
        window.addEventListener('load', cacheDimensions);

        // Call cache immediately once as well
        if (journeySection) {
            cacheDimensions();
        }

        function updateScrollEffects() {
            const scrollY = lastScrollY;

            // Navbar effect - only modify DOM if needed
            if (navbarLayout) {
                if (scrollY > 50) {
                    if (navbarLayout.classList.contains('mt-4')) {
                        navbarLayout.classList.remove('mt-4', 'rounded-full', 'glass');
                        navbarLayout.classList.add('mt-0', 'rounded-b-2xl', 'bg-white/95', 'backdrop-blur-md', 'shadow-md');
                    }
                } else {
                    if (navbarLayout.classList.contains('mt-0')) {
                        navbarLayout.classList.add('mt-4', 'rounded-full', 'glass');
                        navbarLayout.classList.remove('mt-0', 'rounded-b-2xl', 'bg-white/95', 'backdrop-blur-md', 'shadow-md');
                    }
                }
            }

            // Hero parallax effect - Use translate3d for GPU acceleration
            if (heroImg && scrollY <= cachedWindowHeight * 1.5) {
                heroImg.style.transform = `translate3d(0, ${Math.round(scrollY * 0.3)}px, 0) scale(1.05)`;
            }

            // Journey Flow Animation
            if (journeyFlowLine && journeyHeight > 0) {
                // Calculate progress based on pre-cached dimensions to prevent layout thrashing
                const rectTop = journeyTop - scrollY;
                const startThreshold = cachedWindowHeight * 0.6;
                let progress = (startThreshold - rectTop) / journeyHeight;
                progress = Math.max(0, Math.min(1, progress));

                journeyFlowLine.style.transform = `scale3d(1, ${progress}, 1)`;
            }

            // Scroll to Top Button Visibility
            if (scrollToTopBtn) {
                if (scrollY > 500) {
                    if (scrollToTopBtn.classList.contains('translate-y-[100px]')) {
                        scrollToTopBtn.classList.remove('translate-y-[100px]', 'opacity-0', 'pointer-events-none');
                        scrollToTopBtn.classList.add('translate-y-0', 'opacity-100', 'pointer-events-auto');
                    }
                } else {
                    if (!scrollToTopBtn.classList.contains('translate-y-[100px]')) {
                        scrollToTopBtn.classList.add('translate-y-[100px]', 'opacity-0', 'pointer-events-none');
                        scrollToTopBtn.classList.remove('translate-y-0', 'opacity-100', 'pointer-events-auto');
                    }
                }
            }

            // Modal Scroll Logic
            if (window.modalTriggered === false && scrollY > (document.documentElement.scrollHeight * 0.45)) {
                if (window.openRegistrationModal) {
                    window.openRegistrationModal();
                    window.modalTriggered = true;
                }
            }

            ticking = false;
        }

        window.addEventListener('scroll', () => {
            lastScrollY = window.scrollY;
            if (!ticking) {
                window.requestAnimationFrame(updateScrollEffects);
                ticking = true;
            }
        });

        // Add staggered delays to grid items
        document.addEventListener("DOMContentLoaded", () => {
            // Smooth scroll for navigation links
            document.querySelectorAll('a[href^="#"]').forEach(anchor => {
                anchor.addEventListener('click', function (e) {
                    e.preventDefault();
                    const targetId = this.getAttribute('href');
                    if (targetId === '#') return;

                    const targetElement = document.querySelector(targetId);
                    if (targetElement) {
                        targetElement.scrollIntoView({
                            behavior: 'smooth',
                            block: 'start'
                        });
                    }
                });
            });

            // Un-reveal grids and stagger children
            document.querySelectorAll('.grid.reveal').forEach(grid => {
                grid.classList.remove('reveal');
            });

            // Áp dụng Nền Decor Tinh Tế (Luân phiên trái-phải để không đụng nhau)
            // Tạo ra "khoảng thở" và chuyên nghiệp tuyệt đối
            const decors = [
                // 0: Trái - Mầm lá
                `<div class="absolute -left-20 top-1/4 text-primary/10 pointer-events-none select-none scale-[1.5] transform-gpu z-0 opacity-60"><span class="material-symbols-outlined text-[300px]" data-weight="fill">eco</span></div>`,
                // 1: Phải - Rừng thông
                `<div class="absolute -right-24 top-1/3 text-emerald-800/10 -rotate-12 pointer-events-none select-none scale-[1.5] md:scale-[2] transform-gpu z-0 opacity-60"><span class="material-symbols-outlined text-[300px]" data-weight="fill">forest</span></div>`,
                // 2: Trái - Lá năng lượng
                `<div class="absolute -left-24 bottom-20 text-emerald-700/5 rotate-12 pointer-events-none select-none scale-[1.5] md:scale-[2] transform-gpu z-0"><span class="material-symbols-outlined text-[300px]" data-weight="fill">energy_savings_leaf</span></div>`,
                // 3: Phải - Cổ thụ
                `<div class="absolute -right-24 bottom-1/4 text-primary/5 rotate-6 pointer-events-none select-none scale-[1.5] transform-gpu z-0 opacity-60"><span class="material-symbols-outlined text-[300px]" data-weight="fill">park</span></div>`
            ];

            const targetSectionIds = ['difference', 'core-values', 'values-benefits', 'journey', 'values', 'register'];
            targetSectionIds.forEach((id, index) => {
                const section = document.getElementById(id);
                if (section) {
                    if (!section.classList.contains('relative')) section.classList.add('relative');
                    if (!section.classList.contains('overflow-hidden')) section.classList.add('overflow-hidden');
                    
                    // Tránh add 2 lần nếu user F5 nhẹ (bằng cách check element rỗng)
                    const oldDecor = section.querySelector('.material-symbols-outlined.text-\\[300px\\]');
                    if (!oldDecor) {
                        const decorToInject = decors[index % decors.length];
                        section.insertAdjacentHTML('afterbegin', decorToInject);
                    }
                }
            });

            document.querySelectorAll('.grid.reveal').forEach(grid => {
                Array.from(grid.children).forEach((child, index) => {
                    child.classList.add('reveal');
                    if (index > 0) {
                        child.style.transitionDelay = `${(index % 4) * 150}ms`;
                    }
                });
                // Re-observe children
                Array.from(grid.children).forEach(el => observer.observe(el));
            });

            document.querySelectorAll('.hover\\:-translate-y-1, .shadow-sm').forEach(el => {
                if (el.tagName !== 'IMG' && !el.classList.contains('absolute')) {
                    el.classList.add('hover-lift');
                }
            });

            // Modal Logic
            const regModal = document.getElementById('registrationModal');
            const regModalContent = document.getElementById('registrationModalContent');
            window.modalTriggered = false;

            window.openRegistrationModal = function () {
                if (regModal) {
                    regModal.classList.remove('opacity-0', 'pointer-events-none');
                    regModalContent.classList.remove('scale-95');
                    regModalContent.classList.add('scale-100');
                }
            }

            window.closeRegistrationModal = function () {
                if (regModal) {
                    regModal.classList.add('opacity-0', 'pointer-events-none');
                    regModalContent.classList.remove('scale-100');
                    regModalContent.classList.add('scale-95');
                }
            }

            // Auto popup logic after 1s on load
            setTimeout(() => {
                if (!window.modalTriggered) {
                    window.openRegistrationModal();
                    window.modalTriggered = true;
                }
            }, 1000);

            // Intercept CTA buttons pointing to #register
            document.querySelectorAll('a[href="#register"]').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    e.preventDefault();
                    window.openRegistrationModal();
                });
            });
        });
