import { reactive, computed, watch } from 'vue'

// pobieramy stary koszyk z lokalnej pamięci przeglądarki(jesli tam jest)
const savedCart = localStorage.getItem('shopping_cart')

// tworzymy obserwowany magazyn, pobieramy dane jeżeli już jakieś są z przeglądarki lub zwracamy []
export const cartState= reactive({items:savedCart?JSON.parse(savedCart): []})

watch(
    ()=> cartState.items, //pbserwujemy zmiany w tym miejscu

    //co robimy po wykryciu zmian - zamieniamy stary obiekt w localstorage na nowy ze zmianami
    (newItems)=> { localStorage.setItem('shopping_cart', JSON.stringify(newItems))},
    {deep:true}//pilnujemy zmian nie tylko samych itemow ale takze ich wlasciwosci(quantity)
)
//export pozwala innym plikom używać tej wartosci
export const totalPrice= computed(()=>{
    //reduce=redukuje cala liste do jednej liczby
    return cartState.items.reduce((total,item)=>total+item.price *item.quantity,0)
})

export const totalItemsCount=computed(()=>{
    return cartState.items.reduce((total,item)=>total+item.quantity,0)
})

export const cartActions={
    clearCart() {
    cartState.items = []
        },
        

    addToCart(product){
        const existingItem=cartState.items.find(item=>item.id==product.id)
        if (existingItem){
            existingItem.quantity++
        }
        else{
            cartState.items.push({...product,quantity:1})
        }
        console.log("Dodano produkt do koszyka :",product)
    },

    removeFromCart(productId){
        cartState.items=cartState.items.filter(item=>item.id !==productId)
        console.log("Usunięto z koszyka :",productId)
    },

    increaseQuantity(productId){
        const item = cartState.items.find(item=>item.id==productId)
        if (item){
            item.quantity++
        }
    },

    decreaseQuantity(productId){
        const item = cartState.items.find(item=>item.id==productId)
        console.log(item.quantity)
        if (item && item.quantity>1){
            item.quantity--
        }
        else if  (item && item.quantity==1){
            cartActions.removeFromCart(productId)
        }
    },

}
