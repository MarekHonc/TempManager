/**
 * Inicializuje "pan & zoom" interaktivní kotainer, ve kterém se zle pohybovat gesty a myší.
 */
function panAndZoom(containerElementId, contentElementId) {
	const container = document.getElementById(containerElementId);
	const content = document.getElementById(contentElementId);

	let scale = 1;
	let originX = 0;
	let originY = 0;
	let startX = 0;
	let startY = 0;
	let isPanning = false;
	let startDistance = 0;
	let pinchStartScale = 1;

	// Mouse Events for Desktop
	container.addEventListener('wheel', (e) => {
		e.preventDefault();
		const rect = content.getBoundingClientRect();
		const mouseX = e.clientX;// - rect.left;
		const mouseY = e.clientY;// - rect.top;

		const delta = -e.deltaY * 0.001;
		const newScale = Math.min(Math.max(0.5, scale + delta), 3);

		const scaleChange = newScale / scale;
		originX = mouseX - scaleChange * (mouseX - originX);
		originY = mouseY - scaleChange * (mouseY - originY);

		scale = newScale;
		updateTransform();
	});

	container.addEventListener('mousedown', (e) => {
		isPanning = true;
		startX = e.clientX - originX;
		startY = e.clientY - originY;
		container.style.cursor = 'grabbing';
	});

	container.addEventListener('mousemove', (e) => {
		if (!isPanning) return;

		originX = e.clientX - startX;
		originY = e.clientY - startY;
		updateTransform();
	});

	container.addEventListener('mouseup', () => {
		isPanning = false;
		container.style.cursor = 'grab';
	});

	container.addEventListener('mouseleave', () => {
		isPanning = false;
		container.style.cursor = 'grab';
	});

	// Touch Events for Mobile
	container.addEventListener('touchstart', (e) => {
		if (e.touches.length === 1) {
			// Single touch - start panning
			isPanning = true;
			startX = e.touches[0].clientX - originX;
			startY = e.touches[0].clientY - originY;
		} else if (e.touches.length === 2) {
			// Two-finger touch - start pinch-to-zoom
			isPanning = false;
			startDistance = getDistance(e.touches[0], e.touches[1]);
			pinchStartScale = scale;
		}
	});

	container.addEventListener('touchmove', (e) => {
		e.preventDefault();

		if (e.touches.length === 1 && isPanning) {
			// Single touch - pan
			originX = e.touches[0].clientX - startX;
			originY = e.touches[0].clientY - startY;
			updateTransform();
		} else if (e.touches.length === 2) {
			// Two-finger touch - pinch-to-zoom
			const newDistance = getDistance(e.touches[0], e.touches[1]);
			const scaleChange = newDistance / startDistance;
			const newScale = Math.min(Math.max(0.5, pinchStartScale * scaleChange), 3);

			// Calculate the midpoint of the two fingers
			const rect = content.getBoundingClientRect();
			const midX = (e.touches[0].clientX + e.touches[1].clientX) / 2;// - rect.left;
			const midY = (e.touches[0].clientY + e.touches[1].clientY) / 2;// - rect.top;

			const scaleDiff = newScale / scale;
			originX = midX - scaleDiff * (midX - originX);
			originY = midY - scaleDiff * (midY - originY);

			scale = newScale;
			updateTransform();
		}
	});

	container.addEventListener('touchend', () => {
		if (event.touches.length === 0) isPanning = false;
	});

	function getDistance(touch1, touch2) {
		const dx = touch2.clientX - touch1.clientX;
		const dy = touch2.clientY - touch1.clientY;
		return Math.sqrt(dx * dx + dy * dy);
	}

	function updateTransform() {
		content.style.transform = `translate(${originX}px, ${originY}px) scale(${scale})`;
	}
}