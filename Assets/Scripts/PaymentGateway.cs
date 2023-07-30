using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class PaymentGateway : MonoBehaviour, IDetailedStoreListener
{
    public static PaymentGateway ins;
    void Awake() { ins = this; }

    public List<ProductInfo> products;


    IStoreController m_StoreController; // The Unity Purchasing system.

    private Action<string> onProductPurchase;

    [System.Serializable]
    public class ProductInfo
    {
        public string productId;
        public ProductType productType;
    }


    void Start()
    {
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        for (int i = 0; i < products.Count; i++) { builder.AddProduct(products[i].productId, products[i].productType); }
        UnityPurchasing.Initialize(this, builder);
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }


    public void BuyProduct(string productId, Action<string> onProductPurchase)
    {
        this.onProductPurchase = onProductPurchase;
        m_StoreController.InitiatePurchase(productId);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Retrieve the purchased product
        var product = purchaseEvent.purchasedProduct;

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        onProductPurchase?.Invoke(product.definition.id);

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureDescription}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }
}
