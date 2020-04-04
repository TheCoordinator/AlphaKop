/**
 * Event handler that can subscribe to a dispatcher.
 */
export type EventHandler<E> = (event: E) => void

/**
 * Event that can be subscribed to.
 */
export interface Event<E> {
    /**
     * Register a new handler with the dispatcher. Any time the event is
     * dispatched, the handler will be notified.
     * @param handler The handler to register.
     */
    register(handler: EventHandler<E>): void

    /**
     * Desubscribe a handler from the dispatcher.
     * @param handler The handler to remove.
     */
    deregister(handler: EventHandler<E>): void
}
