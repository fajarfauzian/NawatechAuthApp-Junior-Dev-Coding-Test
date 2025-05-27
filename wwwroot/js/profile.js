// Profile Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Elements
    const profileImage = document.getElementById('profileImage');
    const uploadTrigger = document.getElementById('uploadTrigger');
    const profilePictureInput = document.getElementById('profilePictureInput');
    const removeProfileBtn = document.getElementById('removeProfileBtn');
    const resetFormBtn = document.getElementById('resetFormBtn');
    const profileForm = document.getElementById('profile-form');
    const saveChangesBtn = document.getElementById('saveChangesBtn');
    const statusAlert = document.getElementById('statusAlert');
    
    // Image preview functionality
    if (profilePictureInput) {
        profilePictureInput.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                
                reader.onload = function(e) {
                    profileImage.src = e.target.result;
                    
                    // Show loading animation
                    saveChangesBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Uploading...';
                    
                    // Auto-submit the form when image is selected
                    setTimeout(() => {
                        profileForm.submit();
                    }, 500);
                }
                
                reader.readAsDataURL(this.files[0]);
            }
        });
    }
    
    // Trigger file input when clicking on camera icon
    if (uploadTrigger && profilePictureInput) {
        uploadTrigger.addEventListener('click', function() {
            profilePictureInput.click();
        });
    }
    
    // Remove profile picture functionality
    if (removeProfileBtn) {
        removeProfileBtn.addEventListener('click', function() {
            // Create confirmation modal
            const confirmModal = document.createElement('div');
            confirmModal.className = 'modal fade';
            confirmModal.id = 'confirmRemoveModal';
            confirmModal.innerHTML = `
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content border-0 shadow">
                        <div class="modal-header border-0">
                            <h5 class="modal-title">Remove Profile Picture?</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <p>Are you sure you want to remove your profile picture? Your account will use Gravatar instead.</p>
                        </div>
                        <div class="modal-footer border-0">
                            <button type="button" class="btn btn-outline-secondary btn-sm" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-danger btn-sm" id="confirmRemoveBtn">
                                <i class="bi bi-trash me-1"></i>Remove Picture
                            </button>
                        </div>
                    </div>
                </div>
            `;
            
            document.body.appendChild(confirmModal);
            
            // Show modal
            const modal = new bootstrap.Modal(confirmModal);
            modal.show();
            
            // Handle confirmation
            document.getElementById('confirmRemoveBtn').addEventListener('click', function() {
                // Create and submit form to remove profile picture
                const form = document.createElement('form');
                form.method = 'post';
                form.action = '?handler=RemoveProfilePicture';
                
                // Add antiforgery token
                const antiforgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').cloneNode(true);
                form.appendChild(antiforgeryToken);
                
                document.body.appendChild(form);
                form.submit();
            });
            
            // Remove modal after hiding
            confirmModal.addEventListener('hidden.bs.modal', function() {
                document.body.removeChild(confirmModal);
            });
        });
    }
    
    // Reset form functionality
    if (resetFormBtn && profileForm) {
        resetFormBtn.addEventListener('click', function() {
            profileForm.reset();
        });
    }
    
    // Form submission loading state
    if (profileForm) {
        profileForm.addEventListener('submit', function() {
            saveChangesBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Saving...';
            saveChangesBtn.disabled = true;
        });
    }
    
    // Auto-hide status alert after 5 seconds
    if (statusAlert) {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(statusAlert.querySelector('.alert'));
            bsAlert.close();
        }, 5000);
    }
});