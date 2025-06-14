const cookieName = "cart-items";
const isDarkMode = document.documentElement.classList.contains('dark');
const baseUrl = "https://localhost:7279/";
const defaultMinPrice = "0";
const defaultMaxPrice = "35000";
// تنظیمات پیش‌فرض برای حالت دارک و لایت
const alertOptions = {
    background: isDarkMode ? '#333' : '#fff',
    color: isDarkMode ? '#fff' : '#000',
    confirmButtonColor: isDarkMode ? '#4caf50' : '#3085d6',
    cancelButtonColor: isDarkMode ? '#f44336' : '#d33',
};

function addToCart(id, name, unitPrice, unitPriceAfterDiscount, picture,discount) {
    let products = $.cookie(cookieName);
    if (products === undefined) {
        products = [];
    } else {
        products = JSON.parse(products);
    }

    const count = $("#productCount").val();
    const currentProduct = products.find(x => x.id === id);
    if (currentProduct !== undefined) {
        products.find(x => x.id === id).count = count;
        products.find(x => x.id === id).discount = +discount * +count;
        products.find(x => x.id === id).totalPrice = +unitPriceAfterDiscount * +count;

    } else {
        const product = {
            id,
            name,
            unitPrice,
            unitPriceAfterDiscount,
            totalPrice: +unitPriceAfterDiscount * +count,
            picture,
            count,
            discount
        }

        products.push(product);
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function updateCart() {
    let products = $.cookie(cookieName);
    if (products === undefined) { }
    else {
        products = JSON.parse(products);
        $("#cart_items_count").text(products.length);
        const cartItemsWrapper = $("#cart_items_wrapper");
        cartItemsWrapper.html('');

        if (products.length == 0) {
            $("#totalPayAmount").text('0 تومان ');
        }
        products.forEach(x => {
            const product = `<div class="flex items-center gap-x-3 border-b border-gray-200 pb-4 dark:border-white/10">
                                 <!-- IMAGE -->
                                 <img src="/ProductPictures/${x.picture}" class="w-20 h-20 object-cover rounded-lg" alt="" />
                                 <!-- TEXT -->
                                 <div class="flex flex-col gap-y-1">
                                     <h2 class="font-DanaMedium line-clamp-2 text-sm">
                                         ${x.name}
                                     </h2>
                                     <p class="text-xs text-green-600 dark:text-green-500">`+
                                         formatPrice(x.discount * x.count) +`  تومان تخفیف
                                     </p>
                                     <p class="font-DanaDemiBold text-sm">`+
                                         formatPrice(x.totalPrice)+
                                         `<span class="font-Dana"> تومان </span>
                                     </p>
                                     <div class="w-full flex items-center justify-between gap-x-1 rounded-lg border border-gray-200 dark:border-white/20 py-2 px-3" >
                                         <input disabled type="number" name="product_Count" id="product_Count" min="1" max="20" value="${x.count}"
                                                class="custom-input mr-4 text-lg bg-transparent">
                                     <button> 
                                         <svg onclick="alertRemoveFromCart('${x.id}')" class="w-6 h-6 decrement text-red-500">
                                             <use href="#close"></use>
                                         </svg>
                                     </button>
                                     </div>
                                 </div>
                             </div>`;
            cartItemsWrapper.append(product);

            const total = products.reduce((partialSum, a) => partialSum + +a.totalPrice , 0)
            $("#totalPayAmount").text(formatPrice(total)+' تومان ' );


            //var totalItemPrice = $("#TotalItemPrice");
            //totalItemPrice.text();
            //
            //var cartTotalamount = $("#TotalDiscountPrice");
            //cartTotalamount.text();
            //
            //var totalPayPrice = $("#TotalPayPrice");
            //totalPayPrice.text(total);
        })
    }
}

function removeFromCart(id) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.id === id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function removeFromMainCart(id) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.id === id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
    location.reload();
}

function changeCartItemCount(id, totalId, count) {
    var products = $.cookie(cookieName);
    products = JSON.parse(products);
    const productIndex = products.findIndex(x => x.id == id);
    const product = products[productIndex];

    products[productIndex].count = count;
    const newPrice = parseInt(product.unitPriceAfterDiscount) * parseInt(count);
    products[productIndex].totalPrice = newPrice;

    const TotalUnitPrice = products.reduce((partialSum, a) => partialSum + (+a.unitPrice * a.count), 0)
    $("#TotalUnitPrice").text(formatPrice(TotalUnitPrice) + " تومان");

    const TotalDiscountPrice = products.reduce((partialSum, a) => partialSum + (+a.discount * a.count), 0)
    $("#TotalDiscountPrice").text(formatPrice(TotalDiscountPrice) + " تومان");

    const TotalPayPrice = products.reduce((partialSum, a) => partialSum + (+a.unitPriceAfterDiscount * a.count), 0)
    $("#TotalPayPrice").text(formatPrice(TotalPayPrice + " تومان"));

    $("#unitPriceAfterDiscount_" + id).text(formatPrice(product.unitPriceAfterDiscount * product.count) + " تومان");
    var UnitPrice_ = $("#UnitPrice_" + id);
    if (UnitPrice_) {

        UnitPrice_.text(formatPrice(+product.unitPrice * +product.count) + " تومان");
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();



    //const data = {
    //    'productId': parseInt(id),
    //    'count': parseInt(count)
    //};

    //$.ajax({
    //    url: url,
    //    type: "post",
    //    data: JSON.stringify(data),
    //    contentType: "application/json",
    //    dataType: "json",
    //    success: function (data) {
    //        if (data.isInStock == false) {
    //            const warningsDiv = $('#productStockWarnings');
    //            if ($(`#${id}-${colorId}`).length == 0) {
    //                warningsDiv.append(`<div class="alert alert-warning" id="${id}-${colorId}">
    //                    <i class="fa fa-exclamation-triangle"></i>
    //                    <span>
    //                        <strong>${data.productName} - ${color
    //                    } </strong> در حال حاضر در انبار موجود نیست. <strong>${data.supplyDays
    //                    } روز</strong> زمان برای تامین آن نیاز است. ادامه مراحل به منزله تایید این زمان است.
    //                    </span>
    //                </div>
    //                `);
    //            }
    //        } else {
    //            if ($(`#${id}-${colorId}`).length > 0) {
    //                $(`#${id}-${colorId}`).remove();
    //            }
    //        }
    //    },
    //    error: function (data) {
    //        alert("خطایی رخ داده است. لطفا با مدیر سیستم تماس بگیرید.");
    //    }
    //});


    const settings = {
        "url": "https://localhost:5001/api/inventory",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({ "productId": id, "count": count })
    };

    $.ajax(settings).done(function (data) {
        if (data.isStock == false) {
            const warningsDiv = $('#productStockWarnings');
            if ($(`#${id}`).length == 0) {
                warningsDiv.append(`
                    <div class="alert alert-warning" id="${id}">
                        <i class="fa fa-warning"></i> کالای
                        <strong>${data.productName}</strong>
                        در انبار کمتر از تعداد درخواستی موجود است.
                    </div>
                `);
            }
        } else {
            if ($(`#${id}`).length > 0) {
                $(`#${id}`).remove();
            }
        }
    });
}

function addToCartAlert() {
    Swal.fire({
        text: "این کالا به سبد خرید اضافه شد!",
        icon: "success",
        showConfirmButton: false,
        timer: 1500
    });
}

function formatPrice(price) {
    return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function increment(id) {
    id.value = +id.value+1;
    $(id).change();
}
function decrement(id) {
    if (+id.value - 1 < 1) return;
    id.value = +id.value - 1;

    $(id).change();
}
function alertRemoveFromCart(id) {
    Swal.fire({
        title: 'حذف محصول',
        text: 'آیا از حذف این محصول از سبد خرید مطمئن هستید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، حذف کن',
        cancelButtonText: 'لغو',
        ...alertOptions,
    }).then((result) => {
        if (result.isConfirmed) {
            removeFromCart(id);
        }
    });
}
function alertRemoveFromMainCart(id) {
    Swal.fire({
        title: 'حذف محصول',
        text: 'آیا از حذف این محصول از سبد خرید مطمئن هستید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، حذف کن',
        cancelButtonText: 'لغو',
        ...alertOptions,
    }).then((result) => {
        if (result.isConfirmed) {
            removeFromMainCart(id);
        }
    });
}

function search(sort) {
    const minPrice = $('.min-range').val();
    const maxPrice = $('.max-range').val();
    const searchKey = $('.searchKey').val();
    const isExistsChecked = $('.exists').is(':checked');

    const params = new URLSearchParams(); 

    if (searchKey) 
        params.append('searchKey', searchKey);
    
    if (minPrice != defaultMinPrice) 
        params.append('minPrice', minPrice);

    if (maxPrice != defaultMaxPrice) 
        params.append('maxPrice', maxPrice);

    if (isExistsChecked) 
        params.append('exist', 'true');

    $('.categories:checked').each(function () {
        params.append('categories', this.value);
    });

    if (sort) 
        params.append('sort', sort);

    const queryString = params.toString();
    const url = `Search?${queryString}`;

    window.open(`${baseUrl}${url}`, "_self");
}