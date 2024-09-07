new Vue({
    el: '#feedback',
    data: {
        feedbacks: [],
        newReview: {
            Title: '',
            Description: '',
            CountStars: 5
        }
    },
    created() {
        this.updateFeedbacks().then(() => {
            new Swiper('.swiper-container', {
                slidesPerView: 1,
                spaceBetween: 10,
                centeredSlides: true
            });
        });
    },
    methods: {
        async updateFeedbacks() {
            this.feedbacks = [];
            const response = await fetch(`/Feedback/GetFeedbacks`, {
                method: "GET",
            });

            const data = await response.json();

            this.feedbacks = data || [];
        },
        setRating(countStars) {
            this.newReview.CountStars = countStars;
        },
        submitReview() {
            if (this.newReview.Title && this.newReview.Description) {
                this.AddFeedback();
            }
            else {
                Toastify({
                    text: 'Please fill out all fields',
                    duration: 3000
                }).showToast();
            }
        },
        async AddFeedback() {
            const response = await fetch('/Feedback/AddFeedback', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.newReview)
            });
            const result = await response.json();

            if (result.ok) {
                Toastify({
                    text: 'Review submitted successfully!',
                    duration: 3000,
                    style: {
                        background: "green",
                        color: "white"
                    }
                }).showToast();
            }
            this.updateFeedbacks();
        }
    }
});